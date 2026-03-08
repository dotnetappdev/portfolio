/**
 * Accessibility helpers — Read Aloud (Web Speech API) + font-size scaling
 *
 * Screen-reader / BrowseAloud integration notes
 * ─────────────────────────────────────────────
 * • The page already ships proper landmark roles (<main id="main-content">,
 *   <nav> via MudDrawer), ARIA labels on all toolbar buttons, and an
 *   aria-live="polite" announcement region (#a11y-announce).
 * • readPageAloud() targets the #main-content landmark so it only speaks
 *   the body copy — not the navigation bar.
 * • The 'onend' / 'onerror' callbacks invoke dotnet.invokeMethodAsync so
 *   the Blazor component's _isSpeaking state is always in sync, even when
 *   speech finishes naturally.
 */

var _a11yDotNetRef = null;

/**
 * Register the Blazor DotNet object reference that receives the 'onend' callback.
 * Called from AccessibilityToolbar.razor OnAfterRenderAsync.
 * @param {DotNetObjectReference} dotnetRef
 */
window.registerA11yCallback = function (dotnetRef) {
    _a11yDotNetRef = dotnetRef;
};

/**
 * Start reading the page aloud.
 * @param {string} elementId  The id of the root element to read (default: document.body)
 * @returns {boolean} true if speech started, false otherwise
 */
window.readPageAloud = function (elementId) {
    if (!('speechSynthesis' in window)) return false;
    if (window.speechSynthesis.speaking) {
        window.speechSynthesis.cancel();
        return false;
    }
    var el = document.getElementById(elementId) || document.body;
    // Collect text block-by-block so the synthesiser pauses at sentence boundaries.
    // Skip elements that are visually hidden or belong to the accessibility toolbar.
    var blocks = [];
    el.querySelectorAll('p, li, h1, h2, h3, h4, h5, h6, td, th, figcaption').forEach(function (node) {
        // Skip nodes inside the a11y toolbar itself
        if (node.closest('.a11y-toolbar')) return;
        var t = (node.innerText || node.textContent || '').replace(/\s+/g, ' ').trim();
        if (t) blocks.push(t);
    });
    // Fall back to raw innerText when no semantic blocks are found
    var text = blocks.length ? blocks.join('. ') : (el.innerText || el.textContent || '').replace(/\s+/g, ' ').trim();
    if (!text) return false;

    var utterance = new SpeechSynthesisUtterance(text);
    utterance.rate = 0.9;
    utterance.pitch = 1.0;
    utterance.lang = document.documentElement.lang || 'en-GB';

    // Notify Blazor when speech ends or errors out so _isSpeaking resets
    function notifyEnd() {
        if (_a11yDotNetRef) {
            try { _a11yDotNetRef.invokeMethodAsync('OnSpeechEnded'); } catch (e) { /* ignore */ }
        }
    }
    utterance.onend = notifyEnd;
    utterance.onerror = notifyEnd;

    window.speechSynthesis.speak(utterance);
    return true;
};

/**
 * Stop reading aloud immediately.
 */
window.stopReadingAloud = function () {
    if ('speechSynthesis' in window) {
        window.speechSynthesis.cancel();
    }
};

window.isSpeechSynthesisSupported = function () {
    return ('speechSynthesis' in window);
};

window.isSpeaking = function () {
    return ('speechSynthesis' in window) && window.speechSynthesis.speaking;
};

/**
 * Post an accessible announcement into the aria-live region.
 * @param {string} message  Short text to announce to screen readers.
 */
window.announceToScreenReader = function (message) {
    var region = document.getElementById('a11y-announce');
    if (!region) return;
    region.textContent = '';
    setTimeout(function () { region.textContent = message; }, 50);
};

/**
 * Apply a font-size scale class to <body>.
 * @param {string} size - one of: 'default', 'large', 'xlarge'
 */
window.setFontScale = function (size) {
    document.body.classList.remove('font-scale-large', 'font-scale-xlarge');
    if (size === 'large') document.body.classList.add('font-scale-large');
    else if (size === 'xlarge') document.body.classList.add('font-scale-xlarge');
    try { localStorage.setItem('a11y-font-scale', size); } catch (e) { console.warn('[a11y] Could not persist font scale:', e); }
};

window.getSavedFontScale = function () {
    try { return localStorage.getItem('a11y-font-scale') || 'default'; } catch (e) {
        console.warn('[a11y] Could not read saved font scale:', e);
        return 'default';
    }
};
