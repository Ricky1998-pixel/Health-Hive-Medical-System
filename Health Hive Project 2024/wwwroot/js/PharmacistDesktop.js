function showLoading(action) {
    var loadingOverlay = document.getElementById('loadingOverlay');

    // Reset classes to default before applying new ones
    loadingOverlay.className = 'loading-overlay';

    // Example of handling different actions
    switch (action) {
        case 'PharmacistDashboard':
            console.log('Loading Dashboard...');
            // Apply specific class if needed (currently none)
            break;
        case 'ViewPrescriptions':
            console.log('Loading View Prescriptions...');
            // Add custom styles for ViewPrescriptions if needed
            break;
        case 'AddMedications':
            console.log('Loading Add New Medications...');
            // Add custom styles for AddMedications if needed
            break;
        case 'ViewMedications':
            console.log('Loading View Available Medications...');
            // Add custom styles for ViewMedications if needed
            break;
        case 'ViewOrders':
            console.log('Loading View Orders...');
            // Add custom styles for ViewOrders if needed
            break;
        default:
            console.log('Loading...');
    }

    // Show the loading overlay
    loadingOverlay.style.visibility = 'visible';

    // Simulate longer loading time (e.g., 5 seconds)
    setTimeout(function () {
        // Hide the loading overlay after the timeout
        loadingOverlay.style.visibility = 'hidden';
    }, 5000); // Adjusted time to 5 seconds
}

// Hide loading overlay when page is fully loaded
window.addEventListener('load', function () {
    document.getElementById('loadingOverlay').style.visibility = 'hidden';
});




