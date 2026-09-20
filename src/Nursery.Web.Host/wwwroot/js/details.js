  const baseUrl = '@baseUrl';
document.querySelectorAll('.variant-radio').forEach(radio => {
  
            radio.addEventListener('change', function () {
               
                if (!this.checked) return;
               
                // 1. Update text metadata
                document.getElementById('displaySku').innerText = this.dataset.sku;
                document.getElementById('displayRetailPrice').innerText = this.dataset.retail;

                const wholesaleEl = document.getElementById('displayWholesalePrice');
                if (wholesaleEl && this.dataset.wholesale) {
                    wholesaleEl.innerText = this.dataset.wholesale;
                }

                // 2. Parse variant images
                let images = [];
                try {
                    images = JSON.parse(this.dataset.images || '[]');
                } catch (e) {
                    console.error("Failed to parse variant images:", e);
                }

                console.log(images);

                // 3. Update main image & thumbnails
                const thumbnailContainer = document.getElementById('variantThumbnailContainer');
                thumbnailContainer.innerHTML = '';

                if (images.length > 0) {
                    const primary = images.find(x => x.IsPrimaryImage) || images[0];
                    const fullPrimaryUrl = `${baseUrl}${primary.StorageKey}`;

                    const mainImg = document.getElementById('mainVariantImage');
                    mainImg.src = fullPrimaryUrl;
                    mainImg.alt = primary.altText || '';

                    images.forEach((img, idx) => {
                        const fullUrl = `${baseUrl}${img.StorageKey}`;
                        const isDefault = img.StorageKey === primary.StorageKey;

                        const btn = document.createElement('button');
                        btn.type = 'button';
                        btn.className = `btn p-0 border rounded-3 overflow-hidden thumbnail-btn ${isDefault ? 'border-success border-2' : 'border-light-subtle'}`;
                        btn.style.width = '72px';
                        btn.style.height = '72px';
                        btn.onclick = () => switchMainImage(fullUrl, img.altText, btn);

                        btn.innerHTML = `<img src="${fullUrl}" alt="${img.altText || ''}" class="w-100 h-100 object-fit-cover" />`;
                        thumbnailContainer.appendChild(btn);
                    });
                }
            });
        });

        function switchMainImage(url, alt, btnElement) {
            const mainImg = document.getElementById('mainVariantImage');
            mainImg.src = url;
            if (alt) mainImg.alt = alt;

            document.querySelectorAll('.thumbnail-btn').forEach(btn => {
                btn.classList.remove('border-success', 'border-2');
                btn.classList.add('border-light-subtle');
            });
            btnElement.classList.remove('border-light-subtle');
            btnElement.classList.add('border-success', 'border-2');
        }

        function adjustQty(amount) {
            const input = document.getElementById('purchaseQty');
            let current = parseInt(input.value) || 1;
            current = Math.max(1, current + amount);
            input.value = current;
        }

        function addToCart() {
            const selectedRadio = document.querySelector('.variant-radio:checked');
            const id = selectedRadio ? selectedRadio.value : null;
            const qty = parseInt(document.getElementById('purchaseQty').value) || 1;

           console.log(`Adding SKU: ${id}, Quantity: ${qty} to cart.`);

            $.ajax({
                url: '/customer/plants/addtocart',
                type: 'POST',
                data: {
                    plantVariantId: id
                },
                success: function (response) {
                    console.log("Success:", response);
                    // Optional: trigger cart badge/count update UI
                },
                error: function (xhr, error, code) {
                    console.error("Status:", xhr.status);
                    console.error("Response:", xhr.responseText);
                    alert('Failed to add item to cart.');
                }
            });

    
        }