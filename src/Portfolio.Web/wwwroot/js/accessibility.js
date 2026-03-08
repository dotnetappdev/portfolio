/**
 * Accessibility helpers — Read Aloud (Web Speech API) + font-size scaling
 */

window.readPageAloud = function (elementId) {
    if (!('speechSynthesis' in window)) return false;
    if (window.speechSynthesis.speaking) {
        window.speechSynthesis.cancel();
        return false;
    }
    var el = document.getElementById(elementId) || document.body;
    // Collect text block-by-block so the synthesiser pauses at sentence boundaries
    var blocks = [];
    el.querySelectorAll('p, li, h1, h2, h3, h4, h5, h6, td, th, figcaption').forEach(function (node) {
        var t = (node.innerText || node.textContent || '').replace(/\s+/g, ' ').trim();
        if (t) blocks.push(t);
    });
    // Fall back to raw innerText when no semantic blocks are found
    var text = blocks.length ? blocks.join('. ') : (el.innerText || el.textContent || '').replace(/\s+/g, ' ').trim();
    if (!text) return false;
    var utterance = new SpeechSynthesisUtterance(text);
    utterance.rate = 0.9;
    utterance.pitch = 1.0;
    utterance.lang = document.documentElement.lang || 'en';
    window.speechSynthesis.speak(utterance);
    return true;
};

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
