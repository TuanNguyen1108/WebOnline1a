var Ego = {};
Ego.General = {
	init: function() {
		
		Ego.Wishlist.init();
		
		
		Ego.Compare.init();
		
	},
}

Ego.Wishlist = {
	init: function() {
		this.setWishlistProductLoop();
		Ego.Wishlist.wishlistProduct();
	},
	setWishlistProductLoop: function() {
		$('body').on('click', '.remove-wishlist', function(e) {
			Ego.Wishlist.removeWishlist($(this).attr('data-wish'));
		})
		$('body').on('click', '.setWishlist', function(e) {
			//debugger;
			e.preventDefault();
			if ($(this).hasClass('active')) {
				Ego.Wishlist.removeWishlist($(this).attr('data-wish'));
				var InfoText = 'Bạn vừa bỏ sản phẩm ra khỏi mục yêu thích.';
				InfoNoti(InfoText);
			} else {
				var phand = [];
				var handle = $(this).attr('data-wish');
				if (document.cookie.indexOf('ego_wishlist_products') !== -1) {
					var las = Cookies.getJSON('ego_wishlist_products');
					if ($.inArray(handle, las) === -1) {
						phand = [handle];
						for (var i = 0; i < las.length; i++) {
							phand.push(las[i]);
							if (phand.length > 100) {
								break;
							}
						}
						Cookies.set('ego_wishlist_products', phand, {
							expires: 15,
							sameSite: 'None',
							secure: true
						});
					}
				} else {
					phand = [handle];
					Cookies.set('ego_wishlist_products', phand, {
						expires: 15,
						sameSite: 'None',
						secure: true
					});
				}
				Ego.Wishlist.wishlistProduct();
				var SuccessText = "Bạn vừa thêm 1 sản phẩm vào mục yêu thích thành công bấm <a style='color:#2196f3' href='/yeu-thich-san-pham'>vào đây</a> để tới trang yêu thích";
				SuccessNoti(SuccessText);
			}
		})
	},
	wishlistProduct: function() {
		if ($('#sidebar-all .sidebar-all-wrap-right[data-type="wishlist"] .sidebar-all-wrap-right-main-list').length > 0) {
			if (document.cookie.indexOf('ego_wishlist_products') !== -1) {
				$('#sidebar-all .sidebar-all-wrap-right[data-type="wishlist"] .sidebar-all-wrap-right-main-list').html('')
				var last_wishlist_pro_array = Cookies.getJSON('ego_wishlist_products');
				Ego.Wishlist.activityWishlist();
				var recentview_promises = [];
				for (var i = 0; i < 100; i++) {
					if (typeof last_wishlist_pro_array[i] == 'string') {
						var promise = new Promise(function(resolve, reject) {
							$.ajax({
								url: '/products/' + last_wishlist_pro_array[i] + '?view=wish',
								async: false,
								success: function(product) {
									resolve({
										error: false,
										data: product
									});
								},
								error: function(err) {
									if (err.status === 404) {
										try {
											var u = ((this.url.split('?'))[0]).replace('/products/', '');
											resolve({
												error: true,
												handle: u
											});
										} catch (e) {
											resolve({
												error: false,
												data: ''
											})
										}
									} else {
										resolve({
											error: false,
											data: ''
										});
									}
								}
							})
						});
						recentview_promises.push(promise);

					}
				}
				Promise.all(recentview_promises).then(function(values) {
					if (values.length > 0) {
						var x = [];
						setTimeout(function() {
							$('.headerWishlistCount').html(values.length)
						}, 500)
						$.each(values, function(i, v) {
							if (v.error) {
								x.push(v.handle);
							} else {
								$('#sidebar-all .sidebar-all-wrap-right[data-type="wishlist"] .page-wishlist').append(v.data);
								$('#sidebar-all .sidebar-all-wrap-right[data-type="wishlist"] .page-wishlist').show();
								awe_lazyloadImage();
								//theme.init(); // gọi lại ajax cart
							}
							console.log(values.length);

						});
						if (x.length > 0) {
							var new_last_viewed_pro_array = [];
							$.each(last_wishlist_pro_array, function(i, v) {
								if ($.inArray(v, x) === -1) {
									new_last_viewed_pro_array.push(v);
								}

							})
							if (new_last_viewed_pro_array.length > 0) {
								Cookies.set('last_viewed_products', new_last_viewed_pro_array, {
									expires: 180,
									sameSite: 'None',
									secure: true
								});
							}
						}
					} else {
						$('.headerWishlistCount').html('0')
						$('#sidebar-all .sidebar-all-wrap-right[data-type="wishlist"] .sidebar-all-wrap-right-main-list').append('<div class="sidebar-all-wrap-right-main-top-error col-12"><span>Bạn chưa có sản phẩm yêu thích, <a href="/collections/all" style="color: #007bff;">vào đây</a> để thêm thật nhiều sản phẩm vào yêu thích nào. </span></div>')
					}
				});
			} else {
				$('#sidebar-all .sidebar-all-wrap-right[data-type="wishlist"] .sidebar-all-wrap-right-main-list').append('<div class="sidebar-all-wrap-right-main-top-error col-12"><span>Bạn chưa có sản phẩm yêu thích, <a href="/collections/all" style="color: #007bff;">vào đây</a> để thêm thật nhiều sản phẩm vào yêu thích nào. </span></div>')
			}
		} else {
			$('#sidebar-all .sidebar-all-wrap-right[data-type="wishlist"] .sidebar-all-wrap-right-main-list').append('<div class="sidebar-all-wrap-right-main-top-error col-12"><span>Bạn chưa có sản phẩm yêu thích, <a href="/collections/all" style="color: #007bff;">vào đây</a> để thêm thật nhiều sản phẩm vào yêu thích nào. </span></div>')
		}
	},
	activityWishlist: function() {
		var last_wishlist_pro_array = Cookies.getJSON('ego_wishlist_products');
		$.each(last_wishlist_pro_array, function(i, v) {
			$('.setWishlist[data-wish="' + v + '"]').html('<svg xmlns="http://www.w3.org/2000/svg" width="12" height="11" viewBox="0 0 12 11" fill="none"> <path d="M1.21763 5.64124C0.581381 3.86545 1.37671 1.60538 3.12641 1.12108C4.08078 0.798209 5.35327 1.12106 5.98952 2.08967C6.62577 1.12106 7.89826 0.798209 8.85263 1.12108C10.7614 1.76682 11.3977 3.86545 10.7614 5.64124C9.96609 8.38563 6.78483 10 5.98952 10C5.19421 9.83856 2.17201 8.54707 1.21763 5.64124Z" stroke="white" stroke-width="0.75" stroke-linecap="round" stroke-linejoin="round"/> </svg>').addClass('active').attr('title', 'Bỏ yêu thích');
		})
	},

	removeWishlist: function(handle) {
		var phand = [];

		$('a[data-wish="' + handle + '"]').html('<svg xmlns="http://www.w3.org/2000/svg" width="12" height="11" viewBox="0 0 12 11" fill="none"> <path d="M1.21763 5.64124C0.581381 3.86545 1.37671 1.60538 3.12641 1.12108C4.08078 0.798209 5.35327 1.12106 5.98952 2.08967C6.62577 1.12106 7.89826 0.798209 8.85263 1.12108C10.7614 1.76682 11.3977 3.86545 10.7614 5.64124C9.96609 8.38563 6.78483 10 5.98952 10C5.19421 9.83856 2.17201 8.54707 1.21763 5.64124Z" stroke="white" stroke-width="0.75" stroke-linecap="round" stroke-linejoin="round"/> </svg>').removeClass('active').attr('title', 'Thêm vào yêu thích');
		if (document.cookie.indexOf('ego_wishlist_products') !== -1) {
			var las = Cookies.getJSON('ego_wishlist_products');
			var flagIndex = $.inArray(handle, las);
			las.splice(flagIndex, 1)
			Cookies.set('ego_wishlist_products', las, {
				expires: 15,
				sameSite: 'None',
				secure: true
			});
		} else {
			phand = [handle];
			Cookies.set('ego_wishlist_products', phand, {
				expires: 15,
				sameSite: 'None',
				secure: true
			});
		}
		Ego.Wishlist.wishlistProduct(3, 5);
	}
}


$('body').on('click','.btn-show-popup-compare',function(e){
	$('.overplayAll').addClass('active');
	$('.sidebarAllMainCompare').addClass('active');
})
Ego.Compare = {

	init: function() {
		this.compareShow();
		this.compareClose();
		this.setCompareProductLoop();
		this.compareProduct(3, 5);
		this.removeCompare();
	},
	compareShow:function(){
		$('body').on('click','.setCompare:not(.active)',function(e){
			e.preventDefault();
			var phand = [];
			var typeP = [];
			var handle = $(this).data('compare');
			var countCompare = $('.headerCompareCount').text();
			var typeOn = $(this).data('type');
			if (countCompare == '0') {
				typeP = [typeOn];
				Cookies.set('ego_compare_type', typeP, {
					expires: 15,
					sameSite: 'None',
					secure: true
				});
			}
			var storedType = Cookies.getJSON('ego_compare_type');
			if (storedType.includes(typeOn)) {
				$('.overplayAll').addClass('active');
				$('.sidebarAllMainCompare').addClass('active');
			}else {
			}
		})
	},
	compareClose:function(){
		$('body').on('click','.overplayAll, .closeSidebar',function(e){
			e.preventDefault();
			$('.sidebarAllMainCompare').removeClass('active');
			$('.overplayAll').removeClass('active');
		})
	},
	setCompareProductLoop: function() {
		var self = this;
		$('body').on('click', '.setCompare:not(.active)', function(e) {
			e.preventDefault();
			var phand = [];
			var typeP = [];
			var handle = $(this).data('compare');
			var countCompare = $('.headerCompareCount').text();

			var typeOn = $(this).data('type');


			if (countCompare == '0') {
				typeP = [typeOn];
				Cookies.set('ego_compare_type', typeP, {
					expires: 15,
					sameSite: 'None',
					secure: true
				});
			}
			var storedType = Cookies.getJSON('ego_compare_type');

			if (storedType.includes(typeOn)) {
				if (countCompare <= '2') {
					if (document.cookie.indexOf('ego_compare_products') !== -1) {
						var las = Cookies.getJSON('ego_compare_products');
						if ($.inArray(handle, las) === -1) {
							phand = [handle];
							for (var i = 0; i < las.length; i++) {
								phand.push(las[i]);
								if (phand.length > 15) {
									break;
								}
							}
							Cookies.set('ego_compare_products', phand, {
								expires: 15,
								sameSite: 'None',
								secure: true
							});
						}
					} else {
						phand = [handle];
						Cookies.set('ego_compare_products', phand, {
							expires: 15,
							sameSite: 'None',
							secure: true
						});
					}
					self.compareProduct(3, 5);
					self.activityCompare();
					var SuccessText = "Bạn vừa thêm 1 sản phẩm vào mục so sánh thành công bấm <a style='color:#2196f3' href='/so-sanh-san-pham'>vào đây</a> để tới trang so sánh";
					SuccessNoti(SuccessText);
				} else {
					var ErrorText = 'Bạn chỉ có thể so sánh tối đa 3 sản phẩm';
					ErrorNoti(ErrorText);
				}
			}else {
				var ErrorText = 'Sản phẩm so sánh phải cùng loại';
				ErrorNoti(ErrorText);
			}
		})
	},




	compareProduct: function(items, margin) {
		var self = this;


		if (document.cookie.indexOf('ego_compare_products') !== -1) {
			$('.sidebarAllMainCompare .sidebarAllBody').html('');
			var last_compare_pro_array = Cookies.getJSON('ego_compare_products');
			self.activityCompare();
			var recentview_promises = [];
			for (var i = 0; i < 3; i++) {
				if (typeof last_compare_pro_array[i] == 'string') {
					var promise = new Promise(function(resolve, reject) {
						$.ajax({
							url: '/products/' + last_compare_pro_array[i] + '?view=compare',
							async: false,
							success: function(product) {
								resolve({
									error: false,
									data: product
								});
							},
							error: function(err) {
								if (err.status === 404) {
									try {
										var u = ((this.url.split('?'))[0]).replace('/products/', '');
										resolve({
											error: true,
											handle: u
										});
									} catch (e) {
										resolve({
											error: false,
											data: ''
										})
									}
								} else {
									resolve({
										error: false,
										data: ''
									});
								}
							}
						})
					});
					recentview_promises.push(promise);
				}
			}
			Promise.all(recentview_promises).then(function(values) {
				var x = [];
				setTimeout(function() {
					$('.headerCompareCount').html(values.length)
				}, 500)
				$.each(values, function(i, v) {
					if (v.error) {
						x.push(v.handle);
					} else {
						$('.sidebarAllMainCompare .sidebarAllBody').append(v.data);
						$('.sidebarAllMainCompare .sidebarAllBody').show();

					}
				});

			});
			if ($('#pageCompare').length >= 1) {
				var selfPage = $('#pageCompare').find('table');
				for (var i = 0; i < 3; i++) {
					if (typeof last_compare_pro_array[i] == 'string') {
						var promise = new Promise(function(resolve, reject) {
							$.ajax({
								url: '/products/' + last_compare_pro_array[i] + '?view=compare_json',
								async: false,
								success: function(product) {
									//debugger;
									var parseData = $.parseJSON(product);

									selfPage.find(`tr.image td:nth-child(${i + 2})`).html(parseData.image ? `<img class="img-fluid" src="${parseData.image}" alt="${parseData.title}"/>` : "//bizweb.dktcdn.net/thumb/large/assets/themes_support/noimage.gif");
									selfPage.find(`tr.title td:nth-child(${i + 2})`).html(`<h3><a href="${parseData.url}">${parseData.title}</a></h3>`);
									selfPage.find(`tr.price td:nth-child(${i + 2})`).html(parseData.price);
									selfPage.find(`tr.available td:nth-child(${i + 2})`).html(parseData.available);
									selfPage.find(`tr.type td:nth-child(${i + 2})`).html(parseData.type);
									selfPage.find(`tr.vendor td:nth-child(${i + 2})`).html(parseData.vendor);
									selfPage.find(`tr.description td:nth-child(${i + 2})`).html(parseData.description);
								},
								error: function(err) {
									$('#alertError').modal('show').find('.modal-body').html('Xin lỗi, có vấn đề khi thực hiện so sánh, vui lòng thử lại sau!');
								}
							})
						});
						recentview_promises.push(promise);
					}
				}
			}
			setTimeout(function(){
				var countLenght = $('.headerCompareCount').text();
				console.log(countLenght, 'ddd');
				if (countLenght == '0') {
					$('.null-table').removeClass('d-none').addClass('d-block');
					$('.compare-table').addClass('d-none').removeClass('d-block');
					$('.sidebarAllMainCompare .sidebarAllBody').html('<div class="note">Bạn chưa có sản phẩm nào để so sánh hãy thêm vào nhé</div>');
				}else {
					$('.null-table').addClass('d-none').removeClass('d-block');
					$('.compare-table').removeClass('d-none').addClass('d-block');
				}
			},500)
		} 

	},
	activityCompare: function() {


		var last_wishlist_pro_array = Cookies.getJSON('ego_compare_products');
		$.each(last_wishlist_pro_array, function(i, v){
			$('.setCompare[data-compare="' + v + '"]').addClass('active').css('cursor','default').attr('title','Sản phẩm này đã được thêm');
			$('.setCompare[data-compare="' + v + '"]').addClass('active').attr('title', 'Bỏ so sánh');
		})
		setTimeout(function() {
			$('.headerCompareCount').html($('.itemMainCompare').length);
		}, 100)
		setTimeout(function() {
			if ($('.sidebarAllMainCompare .sidebarAllBody').html() ==  ''){
				$('.sidebarAllMainCompare').removeClass('active');
				$('.overplayAll').removeClass('active');
			}
		}, 10)

	},

	removeCompare: function() {
		var self = this;


		$('body').on('click', '.itemMainCompare .removeItem', function(e) {
			e.preventDefault();
			var phand = [];
			var handle = $(this).attr('data-compare');
			var countCompare = $('.headerCompareCount').text();
			$('a[data-compare="' + handle + '"]').removeClass('active').attr('title', 'Thêm vào so sánh');
			if (document.cookie.indexOf('ego_compare_products') !== -1) {
				var las = Cookies.getJSON('ego_compare_products');
				var flagIndex = $.inArray(handle, las);
				las.splice(flagIndex, 1)
				Cookies.set('ego_compare_products', las, {
					expires: 15,
					sameSite: 'None',
					secure: true
				});
			} else {
				Cookies.set('ego_compare_products', phand, {
					expires: 15,
					sameSite: 'None',
					secure: true
				});
			}

			self.compareProduct(3, 5);
			/*location.reload();*/
			if (countCompare == '0') {
				Cookies.set('ego_compare_products', phand, {
					expires: 15,
					sameSite: 'None',
					secure: true
				});
			}		


		})

		$('body').on('click', '.itemMainCompare .removeItem2', function(e) {
			e.preventDefault();
			var phand = [];
			var handle = $(this).attr('data-compare');
			$('a[data-compare="' + handle + '"]').removeClass('active').attr('title', 'Thêm vào so sánh');
			if (document.cookie.indexOf('ego_compare_products') !== -1) {
				var las = Cookies.getJSON('ego_compare_products');
				var flagIndex = $.inArray(handle, las);
				las.splice(flagIndex, 1)
				Cookies.set('ego_compare_products', las, {
					expires: 15,
					sameSite: 'None',
					secure: true
				});
			} else {
				Cookies.set('ego_compare_products', phand, {
					expires: 15,
					sameSite: 'None',
					secure: true
				});
			}

			self.compareProduct(3, 5);
			location.reload();
			if (countCompare == '0') {
				Cookies.set('ego_compare_products', phand, {
					expires: 15,
					sameSite: 'None',
					secure: true
				});
			}		

		})

	}
}




Ego.Wishlist.init();



Ego.Compare.init();