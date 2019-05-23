$(function () {
	$(function () {
		$(".removeProduct").click(function () {
			var recordToDelete = $(this).attr("data-id");
			if (recordToDelete != '') {
				$.post("/Cart/RemoveFromCart", { "productId": recordToDelete },
					function (response) {
						if (response.RemovedItemCount == 0) {
							$('#cart-row-' + response.RemoveItemId).fadeOut('slow', function () {
								if (response.CartItemsCount == 0) {
									$("#cart-empty-message").removeClass("hidden");
								}
							});
						} else {
							$('#cart-item-count-' + response.RemoveItemId).text(response.RemovedItemCount);
						}

						if (response.CartItemsCount == 0) {
							$('#checkout').addClass('hidden');
							$('#total-price').addClass('invisible');
						}

						$('#total-price-value').text(response.CartTotal);
						$('#cart-header-items-count').text(response.CartItemsCount);
					});
				return false;
			}
			return false;
		});
	});
});