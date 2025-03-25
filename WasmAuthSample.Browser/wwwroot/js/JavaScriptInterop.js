globalThis.GetWindowLocation = function () {
    return window.location.href;
}

globalThis.SetWindowLocation = function (location) {
    window.location.href = location;
}