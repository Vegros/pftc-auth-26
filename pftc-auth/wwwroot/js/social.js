(function () {
    const fileInput = document.getElementById('PostImage');
    const selectBtn = document.getElementById('SelectImageBtn');
    const replaceBtn = document.getElementById('ReplaceImageBtn');
    const removeBtn = document.getElementById('RemoveImageBtn');
    const previewContainer = document.getElementById('ImagePreviewContainer');
    const preview = document.getElementById('ImagePreview');
    const fileName = document.getElementById('ImageFileName');
    const imageUrlInput = document.getElementById('ImageUrl');
    const postForm = document.getElementById('postForm');
    const submitBtn = document.getElementById('SubmitPost');

    function showPreview(file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
            fileName.textContent = file.name;
            previewContainer.classList.remove('d-none');
            selectBtn.classList.add('d-none');
        };
        reader.readAsDataURL(file);
    }

    function clearImage() {
        fileInput.value = '';
        imageUrlInput.value = '';
        preview.src = '#';
        fileName.textContent = '';
        previewContainer.classList.add('d-none');
        selectBtn.classList.remove('d-none');
    }

    selectBtn.addEventListener('click', function () {
        fileInput.click();
    });

    replaceBtn.addEventListener('click', function () {
        fileInput.click();
    });

    removeBtn.addEventListener('click', function () {
        clearImage();
    });

    fileInput.addEventListener('change', function () {
        if (fileInput.files && fileInput.files[0]) {
            showPreview(fileInput.files[0]);
            // Clear any previously uploaded URL when a new file is chosen
            imageUrlInput.value = '';
        }
    });

    postForm.addEventListener('submit', async function (e) {
        const file = fileInput.files && fileInput.files[0];

        if (!file) {
            // No image — submit normally
            return;
        }

        // Prevent default submission until image is uploaded
        e.preventDefault();
        submitBtn.disabled = true;
        submitBtn.textContent = 'Uploading...';

        try {
            const formData = new FormData();
            formData.append('file', file);

            const response = await fetch('/Social/UploadImage', {
                method: 'POST',
                body: formData
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || 'Image upload failed.');
            }

            const result = await response.json();
            imageUrlInput.value = result.imageUrl;

            // Remove the file from the file input so it is not sent again
            fileInput.value = '';

            // Submit the form now
            postForm.submit();
        } catch (err) {
            alert('Failed to upload image: ' + err.message);
            submitBtn.disabled = false;
            submitBtn.textContent = 'Post';
        }
    });
})();