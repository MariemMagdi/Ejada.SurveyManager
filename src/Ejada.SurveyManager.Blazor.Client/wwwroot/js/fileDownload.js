// Direct download approach - opens the URL which handles authentication automatically
window.openPdfDownload = function (url) {
    try {
        // Create a temporary link and click it to trigger download
        const link = document.createElement('a');
        link.href = url;
        link.download = ''; // Let the server set the filename via Content-Disposition header
        link.target = '_blank'; // Open in new tab as fallback
        link.style.display = 'none';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    } catch (error) {
        console.error('Error opening PDF download:', error);
        // Fallback: open in new window
        window.open(url, '_blank');
    }
};

// Keep the old function for backward compatibility
window.downloadFile = function (filename, data, contentType) {
    try {
        let blob;
        
        // Check if data is base64 string or byte array
        if (typeof data === 'string') {
            // Base64 string - convert to binary
            const binaryString = window.atob(data);
            const bytes = new Uint8Array(binaryString.length);
            for (let i = 0; i < binaryString.length; i++) {
                bytes[i] = binaryString.charCodeAt(i);
            }
            blob = new Blob([bytes], { type: contentType || 'application/pdf' });
        } else {
            // Byte array - handle different formats
            let bytes;
            if (data instanceof Uint8Array) {
                bytes = data;
            } else if (Array.isArray(data)) {
                bytes = new Uint8Array(data);
            } else if (data && typeof data === 'object' && 'length' in data) {
                bytes = new Uint8Array(Array.from(data));
            } else {
                throw new Error('Invalid data format');
            }
            blob = new Blob([bytes], { type: contentType || 'application/pdf' });
        }
        
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        link.style.display = 'none';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
    } catch (error) {
        console.error('Error downloading file:', error);
        alert('Error downloading file: ' + error.message);
    }
};

