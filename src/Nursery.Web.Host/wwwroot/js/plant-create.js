document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('createPlantForm');
    const submitBtn = document.getElementById('submitBtn');
    const plantImagesContainer = document.getElementById('plantImagesContainer');
    const variantsContainer = document.getElementById('variantsContainer');
    const plantImageTemplate = document.getElementById('plantImageTemplate').innerHTML;
    const salePriceTemplate = document.getElementById('salePriceTemplate').innerHTML;
    const variantTemplate = document.getElementById('variantTemplate').innerHTML;

    let activeUploadCount = 0;

    function toggleSubmitLock(uploading) {
        activeUploadCount += uploading ? 1 : -1;
        if (submitBtn) {
            submitBtn.disabled = activeUploadCount > 0;
            submitBtn.textContent = activeUploadCount > 0 ? 'Uploading files...' : 'Save & Publish Plant';
        }
    }

    // Dynamic Row Ingestion
    document.getElementById('addPlantImageBtn')?.addEventListener('click', () => {
        const nextIdx = plantImagesContainer.querySelectorAll('.image-spec-item').length;
        const html = plantImageTemplate
            .replace(/__PREFIX__/g, 'Images')
            .replace(/__IMG_IDX__/g, nextIdx);
        plantImagesContainer.insertAdjacentHTML('beforeend', html);
    });

    document.getElementById('addVariantBtn')?.addEventListener('click', () => {
        const nextIdx = variantsContainer.querySelectorAll('.variant-card').length;
        const html = variantTemplate
            .replace(/__VAR_IDX__/g, nextIdx)
            .replace(/__DISPLAY_IDX__/g, nextIdx + 1);
        variantsContainer.insertAdjacentHTML('beforeend', html);
    });

    // Form Event Delegations
    form.addEventListener('click', (e) => {
        if (e.target.closest('.remove-row-btn')) {
            const row = e.target.closest('.image-spec-item');
            const parent = row.parentElement;
            row.remove();
            reindexImageCollection(parent);
            return;
        }
        if (e.target.closest('.remove-sale-btn')) {
            const row = e.target.closest('.sale-price-item');
            const parent = row.parentElement;
            row.remove();
            reindexsales(parent);
            return;
        }


        if (e.target.closest('.remove-variant-btn')) {
            const card = e.target.closest('.variant-card');
            card.remove();
            reindexVariants();
            return;
        }

     
 

        const addVarImgBtn = e.target.closest('.add-variant-image-btn');
        if (addVarImgBtn) {
            const variantCard = addVarImgBtn.closest('.variant-card');
            const varIdx = variantCard.dataset.index;
            const container = variantCard.querySelector('.variant-image-container');
            const imgIdx = container.querySelectorAll('.image-spec-item').length;
            console.log(variantCard.dataset);
            const html = plantImageTemplate
                .replace(/__PREFIX__/g, `PlantVariantSpecs[${varIdx}].ImageSpecs`)
                .replace(/__IMG_IDX__/g, imgIdx);

            container.insertAdjacentHTML('beforeend', html);
        }

        const addSalePriceBtn = e.target.closest('.add-Sale-btn');
        if (addSalePriceBtn) {
            const salePriceCard = addSalePriceBtn.closest('.variant-card');
            const varIdx = salePriceCard.dataset.index;
            const container = salePriceCard.querySelector('.sale-price-container');
            const spIdx = container.querySelectorAll('.sale-price-item').length;
            // console.log(salePriceCard.dataset);
            const html = salePriceTemplate
                .replace(/__PREFIX__/g, `PlantVariantSpecs[${varIdx}].SalePrices`)
                .replace(/__SALE_DISPLAY_IDX__/g, spIdx + 1)
                .replace(/__SP_IDX__/g, spIdx)
                .replace(/varIdx/g, varIdx);

            container.insertAdjacentHTML('beforeend', html);
        }

    });

    // Async Image Upload Pipeline
    form.addEventListener('change', async (e) => {
        const fileInput = e.target.closest('.image-file-input');
        if (!fileInput || !fileInput.files || fileInput.files.length === 0) return;

        const file = fileInput.files[0];
        const row = fileInput.closest('.image-spec-item');
        const spinner = row.querySelector('.upload-status');
        const hiddenKeyInput = row.querySelector('.storage-key-input');
        const badgeDisplay = row.querySelector('.storage-key-display');

        const formData = new FormData();
        formData.append('file', file);

        try {
            toggleSubmitLock(true);
            spinner.classList.remove('d-none');
            fileInput.disabled = true;

            const response = await fetch('/admin/plants/upload-image', {
                method: 'POST',
                body: formData
            });

            if (!response.ok) {
                throw new Error(`Upload failed with status: ${response.status}`);
            }

            const result = await response.json();
            // Supports { storageKey: "..." } or { data: { storageKey: "..." } }
            const storageKey = result.storageKey || result.data?.storageKey || result.key;

            if (!storageKey) {
                throw new Error('API did not return a valid storageKey');
            }

            // Assign key to hidden input for MVC binder
            hiddenKeyInput.value = storageKey;

            // UI feedback
            badgeDisplay.textContent = `Key: ${storageKey}`;
            badgeDisplay.classList.remove('d-none');
            badgeDisplay.classList.replace('bg-secondary', 'bg-success');
        } catch (err) {
            console.error('Image upload failed:', err);
            alert('Image upload failed. Please try again.');
            fileInput.value = '';
            hiddenKeyInput.value = '';
            badgeDisplay.classList.add('d-none');
        } finally {
            spinner.classList.add('d-none');
            fileInput.disabled = false;
            toggleSubmitLock(false);
        }
    });

    // Reindexing helpers for ASP.NET MVC model binder
    function reindexImageCollection(container) {
        const items = container.querySelectorAll('.image-spec-item');
        items.forEach((item, idx) => {
            item.dataset.index = idx;
   

            item.querySelectorAll('input').forEach(input => {
                input.name = input.name.replace(/\[\d+\](?=\.[^[]+$)/, `[${idx}]`);
            });
        });
    }

    function reindexsales(container) {

        const items = container.querySelectorAll('.sale-price-item');
        items.forEach((item, idx) => {
            item.dataset.index = idx;

            const displayBadge = item.querySelector('.sale-display-idx');
            console.log(displayBadge);
            if (displayBadge) displayBadge.textContent = idx + 1;

            item.querySelectorAll('input').forEach(input => {
                input.name = input.name.replace(/\[\d+\](?=\.[^[]+$)/, `[${idx}]`);
            });
        });
    }

    function reindexVariants() {
        const variants = variantsContainer.querySelectorAll('.variant-card');
        variants.forEach((card, vIdx) => {
            card.dataset.index = vIdx;
            const displayBadge = card.querySelector('.variant-display-idx');
            if (displayBadge) displayBadge.textContent = vIdx + 1;

            card.querySelectorAll('input').forEach(input => {
                input.name = input.name.replace(/^PlantVariantSpecs\[\d+\]/, `PlantVariantSpecs[${vIdx}]`);
            });

            const addImgBtn = card.querySelector('.add-variant-image-btn');
            if (addImgBtn) {
                addImgBtn.dataset.variantPrefix = `PlantVariantSpecs[${vIdx}]`;
            }

            const imgContainer = card.querySelector('.variant-image-container');
            if (imgContainer) {
                reindexImageCollection(imgContainer);
            }

            const addSaleBtn = card.querySelector('.add-sale-btn');
            if (addSaleBtn) {
                addSaleBtn.dataset.variantPrefix = `PlantVariantSpecs[${vIdx}]`;
            }

            const saleContainer = card.querySelector('.sale-price-container');
            if (saleContainer) {
                reindexsales(saleContainer);
            }
        });
    }

});