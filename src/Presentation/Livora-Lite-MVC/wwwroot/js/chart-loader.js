// Chart.js Loader with Fallback Support
// Ensures Chart is available globally before executing chart code

(function() {
    'use strict';

    window.ChartLoader = {
        isReady: false,
        callbacks: [],
        
        readyCallbacks: [],
        
        onChartReady: function(callback) {
            if (this.isReady && typeof window.Chart !== 'undefined') {
                callback();
            } else {
                this.readyCallbacks.push(callback);
            }
        },
        
        setReady: function() {
            this.isReady = true;
            this.readyCallbacks.forEach(cb => {
                try {
                    cb();
                } catch (e) {
                    console.error('Error in Chart ready callback:', e);
                }
            });
        }
    };

    // Listen for Chart.js ready event from _Layout.cshtml
    document.addEventListener('chartjsReady', function() {
        window.ChartLoader.setReady();
    });

    // Fallback: Check if Chart is already loaded globally
    setTimeout(function() {
        if (typeof window.Chart !== 'undefined' && !window.ChartLoader.isReady) {
            window.ChartLoader.setReady();
        }
    }, 2000);

    // Create a wrapper for new Chart() that waits until Chart is ready
    var ChartWrapper = function(canvasContext, config) {
        if (typeof window.Chart === 'undefined') {
            console.error('Chart.js not loaded yet. Retrying in 500ms...');
            setTimeout(() => {
                new window.Chart(canvasContext, config);
            }, 500);
        } else {
            return new window.Chart(canvasContext, config);
        }
    };

    // Override global Chart if it's not yet available
    if (typeof window.Chart === 'undefined') {
        window.Chart = ChartWrapper;
    }

})();
