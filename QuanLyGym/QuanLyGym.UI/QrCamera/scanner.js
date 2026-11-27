const devices = document.getElementById("devices");
const start = document.getElementById("start");
const statusLabel = document.getElementById("status");
const placeholder = document.getElementById("placeholder");
const reader = document.getElementById("reader");
const viewport = reader.parentElement;
let scanner;

function fitPreview() {
    const video = reader.querySelector("video");
    if (!video?.clientWidth || !video.clientHeight) return;
    // Transform only the preview so decoding keeps its original region and dimensions.
    const scale = Math.min(viewport.clientWidth / video.clientWidth, viewport.clientHeight / video.clientHeight);
    reader.style.transform = 'scale(' + scale + ')';
}
new ResizeObserver(fitPreview).observe(viewport);
let lastCode = "";
let lastSeenAt = 0;

function waitForVideoFrame(video) {
    return new Promise((resolve, reject) => {
        const callback = video.requestVideoFrameCallback(() => {
            clearTimeout(timer);
            resolve();
        });
        const timer = setTimeout(() => {
            video.cancelVideoFrameCallback(callback);
            reject(new Error("CameraFrameTimeout"));
        }, 4500);
    });
}

function checkForDarkImage(video) {
    const canvas = document.createElement("canvas");
    canvas.width = 16;
    canvas.height = 12;
    const context = canvas.getContext("2d", { willReadFrequently: true });
    if (!context) return false;
    context.drawImage(video, 0, 0, canvas.width, canvas.height);
    const { data } = context.getImageData(0, 0, canvas.width, canvas.height);
    for (let index = 0; index < data.length; index += 4) {
        if (Math.max(data[index], data[index + 1], data[index + 2]) > 16) return false;
    }
    return true;
}

async function refreshDevices(video) {
    try {
        const activeId = video.srcObject?.getVideoTracks()[0]?.getSettings().deviceId;
        const cameras = (await navigator.mediaDevices.enumerateDevices())
            .filter(device => device.kind === "videoinput" && device.deviceId);
        if (!video.isConnected || !scanner.isScanning || !cameras.length) return;
        devices.replaceChildren(...cameras.map((camera, index) =>
            new Option(camera.label || "Camera " + (index + 1), camera.deviceId)));
        if (cameras.some(camera => camera.deviceId === activeId)) devices.value = activeId;
        devices.disabled = start.disabled;
    } catch {
        // A working video stream does not depend on the device list.
    }
}


async function toggleCamera(changeDevice = false) {
    if (start.disabled) return;
    start.disabled = true;
    devices.disabled = true;
    try {
        const wasScanning = scanner?.isScanning === true;
        if (wasScanning) await scanner.stop();
        reader.style.removeProperty("width");
        reader.style.removeProperty("transform");
        if (wasScanning && !changeDevice) {
            statusLabel.textContent = "";
            return;
        }

        statusLabel.textContent = "Đang mở camera…";
        if (!scanner) {
            scanner = new Html5Qrcode("reader", {
                formatsToSupport: [Html5QrcodeSupportedFormats.QR_CODE],
                useBarCodeDetectorIfSupported: false
            });
        }
        lastCode = "";
        const camera = devices.options[0]?.value ? devices.value : { facingMode: "user" };
        reader.style.width = reader.clientWidth + "px";
        await scanner.start(camera, {
            fps: 12,
            aspectRatio: 4 / 3,
            qrbox: (width, height) => {
                const side = Math.floor(Math.min(width, height) * 0.86);
                return { width: side, height: side };
            }
        }, raw => {
            const code = raw.trim();
            if (!code || code.length > 80) return;
            const now = Date.now();
            const repeat = code === lastCode && now - lastSeenAt < 1200;
            lastCode = code;
            lastSeenAt = now;
            if (!repeat) {
                statusLabel.textContent = "Đã đọc mã QR. Xem kết quả check-in ở bên phải.";
                window.chrome.webview.postMessage(code);
            }
        }, () => {});
        const video = reader.querySelector("video");
        if (!video) throw new Error("CameraFrameTimeout");
        fitPreview();
        placeholder.hidden = true;
        void refreshDevices(video);
        await waitForVideoFrame(video);
        if (!lastCode) statusLabel.textContent = "Đưa toàn bộ mã QR vào khung và giữ yên.";
        setTimeout(() => {
            if (scanner.isScanning && !lastCode && video.isConnected && checkForDarkImage(video)) {
                statusLabel.textContent = "Camera đang trả hình tối. Kiểm tra nắp che, ánh sáng hoặc chọn camera khác.";
            }
        }, 900);
    } catch (error) {
        if (scanner?.isScanning) {
            try { await scanner.stop(); } catch { /* Keep the original error visible. */ }
        }
        reader.style.removeProperty("width");
        reader.style.removeProperty("transform");
        if (error.message === "CameraFrameTimeout") {
            statusLabel.textContent = "Camera đã mở nhưng chưa có hình. Hãy thử tắt ứng dụng đang dùng webcam hoặc chọn camera khác.";
        } else if (error.name === "NotAllowedError") {
            statusLabel.textContent = "Chưa cấp quyền camera. Kiểm tra Cài đặt Windows → Quyền riêng tư → Camera.";
        } else {
            statusLabel.textContent = "Không mở được camera. Kiểm tra kết nối, quyền camera hoặc thử camera khác.";
        }
    } finally {
        const scanning = scanner?.isScanning === true;
        placeholder.hidden = scanning;
        start.textContent = scanning ? "Tắt camera" : "Bật camera";
        devices.disabled = !devices.options[0]?.value;
        start.disabled = false;
    }
}

start.addEventListener("click", () => toggleCamera());
devices.addEventListener("change", () => {
    if (scanner?.isScanning) toggleCamera(true);
});
