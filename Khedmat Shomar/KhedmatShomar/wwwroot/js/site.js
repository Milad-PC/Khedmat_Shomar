// خدمت‌شمار — تم، شمارنده زنده، مودال نصب PWA
(function () {
    'use strict';

    function fa(n) {
        return String(n).replace(/\d/g, function (x) { return '۰۱۲۳۴۵۶۷۸۹'[+x]; });
    }

    /* ---------- تم ---------- */
    var root = document.documentElement;
    var themeLabel = document.getElementById('themeLabel');

    function currentTheme() {
        return root.getAttribute('data-theme') === 'light' ? 'light' : 'dark';
    }

    function syncThemeUi() {
        if (themeLabel) themeLabel.textContent = currentTheme() === 'dark' ? 'روشن' : 'تیره';
        var meta = document.querySelector('meta[name="theme-color"]');
        if (meta) meta.setAttribute('content', currentTheme() === 'dark' ? '#141710' : '#eceadd');
    }

    var themeToggle = document.getElementById('themeToggle');
    if (themeToggle) {
        themeToggle.addEventListener('click', function () {
            var next = currentTheme() === 'dark' ? 'light' : 'dark';
            if (next === 'light') root.setAttribute('data-theme', 'light');
            else root.removeAttribute('data-theme');
            try { localStorage.setItem('ks_theme', next); } catch (e) { }
            syncThemeUi();
        });
    }
    syncThemeUi();

    /* ---------- شمارنده زنده ---------- */
    var counterCard = document.getElementById('counterCard');
    if (counterCard) {
        var finishMs = Date.parse(counterCard.dataset.finish);
        var startMs = Date.parse(counterCard.dataset.start);
        var CIRC = 552.9;

        var refresh = function () {
            if (isNaN(finishMs)) return;
            var now = Date.now();
            var remain = Math.max(0, Math.ceil((finishMs - now) / 864e5));
            var total = Math.max(1, finishMs - startMs);
            var pct = Math.min(100, Math.max(0, 100 * (1 - (finishMs - now) / total)));

            var el;
            if ((el = document.getElementById('remainDays'))) el.textContent = fa(remain);
            if ((el = document.getElementById('pctLine'))) el.textContent = '٪' + fa(Math.round(pct)) + ' از خدمت طی شده';
            if ((el = document.getElementById('ringProgress'))) el.setAttribute('stroke-dasharray', (pct / 100 * CIRC).toFixed(1) + ' ' + CIRC);
            if ((el = document.getElementById('bdYears'))) el.textContent = fa(Math.floor(remain / 365));
            if ((el = document.getElementById('bdMonths'))) el.textContent = fa(Math.floor(remain % 365 / 30));
            if ((el = document.getElementById('bdDays'))) el.textContent = fa(remain % 365 % 30);
        };

        refresh();
        setInterval(refresh, 30000);
    }

    /* ---------- PWA: سرویس‌ورکر ---------- */
    if ('serviceWorker' in navigator) {
        navigator.serviceWorker.register('/sw.js').catch(function () { });
    }

    /* ---------- PWA: مودال نصب ---------- */
    var modal = document.getElementById('installModal');
    var installBtn = document.getElementById('installAction');
    var deferredPrompt = null;

    function isStandalone() {
        return window.matchMedia('(display-mode: standalone)').matches
            || window.navigator.standalone === true;
    }

    function openModal() {
        if (!modal) return;
        modal.hidden = false;
        if (installBtn) installBtn.hidden = !deferredPrompt;
    }

    function closeModal() {
        if (modal) modal.hidden = true;
    }

    window.addEventListener('beforeinstallprompt', function (e) {
        e.preventDefault();
        deferredPrompt = e;
        if (modal && !modal.hidden && installBtn) installBtn.hidden = false;
    });

    window.addEventListener('appinstalled', function () {
        deferredPrompt = null;
        closeModal();
    });

    var openLink = document.getElementById('openInstallModal');
    if (openLink) openLink.addEventListener('click', openModal);

    var closeBtn = document.getElementById('closeInstallModal');
    if (closeBtn) closeBtn.addEventListener('click', closeModal);

    if (modal) {
        modal.addEventListener('click', function (e) {
            if (e.target === modal) closeModal();
        });
    }

    if (installBtn) {
        installBtn.addEventListener('click', function () {
            if (!deferredPrompt) return;
            deferredPrompt.prompt();
            deferredPrompt.userChoice.finally(function () {
                deferredPrompt = null;
                closeModal();
            });
        });
    }

    // بعد از اولین محاسبه (اولین بار دیدن صفحه شمارنده) فقط یک‌بار خودکار نمایش بده
    if (counterCard && !isStandalone()) {
        var shown = null;
        try { shown = localStorage.getItem('ks_installShown'); } catch (e) { }
        if (!shown) {
            try { localStorage.setItem('ks_installShown', '1'); } catch (e) { }
            // کمی تاخیر تا beforeinstallprompt فرصت شلیک داشته باشد
            setTimeout(openModal, 600);
        }
    }
})();
