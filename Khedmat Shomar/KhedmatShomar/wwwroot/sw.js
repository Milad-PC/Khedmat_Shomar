// خدمت‌شمار — Service Worker: کش شل اپ برای کار آفلاین
const CACHE = 'ks-shell-v1';
const SHELL = [
    '/',
    '/css/site.css',
    '/js/site.js',
    '/manifest.webmanifest',
    '/icons/icon.svg',
    '/icons/icon-192.png',
    '/icons/icon-512.png'
];

self.addEventListener('install', function (e) {
    e.waitUntil(
        caches.open(CACHE).then(function (c) { return c.addAll(SHELL); }).then(function () { return self.skipWaiting(); })
    );
});

self.addEventListener('activate', function (e) {
    e.waitUntil(
        caches.keys().then(function (keys) {
            return Promise.all(keys.filter(function (k) { return k !== CACHE; }).map(function (k) { return caches.delete(k); }));
        }).then(function () { return self.clients.claim(); })
    );
});

self.addEventListener('fetch', function (e) {
    var req = e.request;
    if (req.method !== 'GET') return;

    // ناوبری: اول شبکه (کوکی تعیین می‌کند فرم یا شمارنده)، آفلاین → کش
    if (req.mode === 'navigate') {
        e.respondWith(
            fetch(req).then(function (res) {
                var copy = res.clone();
                caches.open(CACHE).then(function (c) { c.put('/', copy); });
                return res;
            }).catch(function () {
                return caches.match('/');
            })
        );
        return;
    }

    // استاتیک و فونت: اول کش، بعد شبکه (و ذخیره در کش)
    e.respondWith(
        caches.match(req).then(function (hit) {
            return hit || fetch(req).then(function (res) {
                if (res.ok && (req.url.startsWith(self.location.origin) || req.url.indexOf('fonts.g') !== -1)) {
                    var copy = res.clone();
                    caches.open(CACHE).then(function (c) { c.put(req, copy); });
                }
                return res;
            });
        })
    );
});
