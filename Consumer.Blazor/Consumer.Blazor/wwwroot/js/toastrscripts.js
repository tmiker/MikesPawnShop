function ShowToastr(type, message) {
    if (type === "success") {
        // Override global options
        toastr.success(message, 'Operation Successful', { timeOut: 5000 });
    }
    if (type === "error") {
        // Override global options
        toastr.error(message, 'Operation Failed', { timeOut: 5000 });
    }
    if (type === "info") {
        // Override global options
        toastr.info(message, 'Information', { timeOut: 5000 });
    }
}
