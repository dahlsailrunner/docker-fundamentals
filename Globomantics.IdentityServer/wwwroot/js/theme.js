jQuery( document ).ready( function( $ ) {
	'Use Strict';

	/**
	 *	Table Of Content
	 *	1. Contact Us Page
	 *	2. About Us Page
	 *	3. Property Listing Page
	 *	4. Property Detail Page
	 *	5. Search Popup
	 *	6. Scroll Header Mobile
	 *	7. Blog List Page
	 *	8. Blog Grid Page
	 *	9. Homepage
	 *	10. Homepage Map
	 *
	 * 	Fire all js function.
	 */

	/**
	 * 1. Contact Us Page
	 *
	 * @author	lox
	 */
	function homeline_contact_us_page() {
		if ( !( $( 'body' ).hasClass( 'contact-us-page' ) ) ) return false;

		var map = new google.maps.Map( document.getElementById( 'contact_map' ), {
			center: { lat: 40.643747, lng: -73.930104 },
			zoom: 12,
			draggable: false,
			scrollwheel: false,
			styles: [
				{
					"elementType": "geometry",
					"stylers": [ {
						"color": "#f5f5f5"
					} ]
				},
				{
					"elementType": "labels.icon",
					"stylers": [ {
						"visibility": "off"
					} ]
				},
				{
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#616161"
					} ]
				},
				{
					"elementType": "labels.text.stroke",
					"stylers": [ {
						"color": "#f5f5f5"
					} ]
				},
				{
					"featureType": "administrative.land_parcel",
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#bdbdbd"
					} ]
				},
				{
					"featureType": "poi",
					"elementType": "geometry",
					"stylers": [ {
						"color": "#eeeeee"
					} ]
				},
				{
					"featureType": "poi",
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#757575"
					} ]
				},
				{
					"featureType": "poi.park",
					"elementType": "geometry",
					"stylers": [ {
						"color": "#e5e5e5"
					} ]
				},
				{
					"featureType": "poi.park",
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#9e9e9e"
					} ]
				},
				{
					"featureType": "road",
					"elementType": "geometry",
					"stylers": [ {
						"color": "#ffffff"
					} ]
				},
				{
					"featureType": "road.arterial",
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#757575"
					} ]
				},
				{
					"featureType": "road.highway",
					"elementType": "geometry",
					"stylers": [ {
						"color": "#dadada"
					} ]
				},
				{
					"featureType": "road.highway",
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#616161"
					} ]
				},
				{
					"featureType": "road.local",
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#9e9e9e"
					} ]
				},
				{
					"featureType": "transit.line",
					"elementType": "geometry",
					"stylers": [ {
						"color": "#e5e5e5"
					} ]
				},
				{
					"featureType": "transit.station",
					"elementType": "geometry",
					"stylers": [ {
						"color": "#eeeeee"
					} ]
				},
				{
					"featureType": "water",
					"elementType": "geometry",
					"stylers": [ {
						"color": "#c9c9c9"
					} ]
				},
				{
					"featureType": "water",
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#9e9e9e"
					} ]
				}
			]
		} );

		var marker = new google.maps.Marker( {
			position: { lat: 40.643747, lng: -73.930104 },
			icon: './assets/img/map-marker.png',
			map: map
		} );
	}

	/**
	 * 2. About Us Page
	 *
	 * @author	lox
	 */
	function homeline_about_us_page() {
		if ( !( $( 'body' ).hasClass( 'about-us-page' ) ) ) return false;

		$( '.counter-value' ).counterUp( {
			delay: 10,
			time: 1000
		} );
	}

	/**
	 * 3. Property Listing Page
	 *
	 * @author	lox
	 */
	function homeline_property_listing_page() {
		if ( !( $( 'body' ).hasClass( 'property-listing-page' ) ) ) return false;

		$( '.menu-filter-property ul' ).each( function() {
			var child_list = $( this ).children( 'li' );
			child_list.on( 'click', function( e ) {
				e.preventDefault();
				if ( child_list.hasClass( 'active' ) ) {
					child_list.removeClass( 'active' );
					$( this ).addClass( 'active' );
				}
			} );
		} );
	}

	/**
	 * 4. Property Detail Page
	 *
	 * @author	lox
	 */
	function homeline_property_detail_page() {
		if ( !( $( 'body' ).hasClass( 'single-property-page' ) ) ) return false;

		$( '.slider-range' ).slider( {
			range: true,
			min: 0,
			max: 15000,
			values: [ 2500, 8750 ],
			slide: function( event, ui ) {
				$( '.amount' ).val( '$' + ui.values[ 0 ] + ' - $' + ui.values[ 1 ] );
			}
		} );
		$( '.amount' ).val( '$' + $( '.slider-range' ).slider( 'values', 0 ) + ' - $' + $( '.slider-range' ).slider( 'values', 1 ) );

		$( '.owl-carousel' ).owlCarousel( {
			loop: true,
			margin: 10,
			nav: false,
			items: 1,
			thumbs: true,
			thumbsPrerendered: true,
			autoplay: false,
			autoplayTimeout: 5000,
			autoplayHoverPause: false
		} );

		$( 'iframe' ).parent().fitVids();

		var map = new google.maps.Map( document.getElementById( 'single-property-map' ), {
			center: { lat: 40.643747, lng: -73.930104 },
			zoom: 12,
			draggable: false,
			scrollwheel: false,
			styles: [
				{
					"elementType": "geometry",
					"stylers": [ {
						"color": "#f5f5f5"
					} ]
				},
				{
					"elementType": "labels.icon",
					"stylers": [ {
						"visibility": "off"
					} ]
				},
				{
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#616161"
					} ]
				},
				{
					"elementType": "labels.text.stroke",
					"stylers": [ {
						"color": "#f5f5f5"
					} ]
				},
				{
					"featureType": "administrative.land_parcel",
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#bdbdbd"
					} ]
				},
				{
					"featureType": "poi",
					"elementType": "geometry",
					"stylers": [ {
						"color": "#eeeeee"
					} ]
				},
				{
					"featureType": "poi",
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#757575"
					} ]
				},
				{
					"featureType": "poi.park",
					"elementType": "geometry",
					"stylers": [ {
						"color": "#e5e5e5"
					} ]
				},
				{
					"featureType": "poi.park",
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#9e9e9e"
					} ]
				},
				{
					"featureType": "road",
					"elementType": "geometry",
					"stylers": [ {
						"color": "#ffffff"
					} ]
				},
				{
					"featureType": "road.arterial",
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#757575"
					} ]
				},
				{
					"featureType": "road.highway",
					"elementType": "geometry",
					"stylers": [ {
						"color": "#dadada"
					} ]
				},
				{
					"featureType": "road.highway",
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#616161"
					} ]
				},
				{
					"featureType": "road.local",
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#9e9e9e"
					} ]
				},
				{
					"featureType": "transit.line",
					"elementType": "geometry",
					"stylers": [ {
						"color": "#e5e5e5"
					} ]
				},
				{
					"featureType": "transit.station",
					"elementType": "geometry",
					"stylers": [ {
						"color": "#eeeeee"
					} ]
				},
				{
					"featureType": "water",
					"elementType": "geometry",
					"stylers": [ {
						"color": "#c9c9c9"
					} ]
				},
				{
					"featureType": "water",
					"elementType": "labels.text.fill",
					"stylers": [ {
						"color": "#9e9e9e"
					} ]
				}
			]
		} );

		var marker = new google.maps.Marker( {
			position: { lat: 40.643747, lng: -73.930104 },
			icon: './assets/img/map-marker.png',
			map: map
		} );

		$( '#accordion' ).accordion();
	}

	/**
	 * 5. Search Popup
	 *
	 * @author	lox
	 */
	function homeline_search_popup() {
		$( '.menu-search' ).on( 'click', function( e ) {
			e.preventDefault();
			$( '#search' ).addClass( 'open' );
		} );

		$( '#search .close' ).on( 'click', function( e ) {
			e.preventDefault();
			$( '#search' ).removeClass( 'open' );
		} );
	}

	/**
	 * 6. Scroll Header Mobile
	 *
	 * @author	lox
	 */
	function homeline_scroll_header_mobile() {

		if ( $( window ).width() < 769 && $( window ).width() > 601 ) {
			$( '<div class="show-submenu"><i class="fa fa-chevron-circle-down"></i></div>' ).appendTo( '.has-menu-item' );
			$( '<div class="sidebar-menu-overlay"></div>' ).appendTo( '.page-site' );
			$( '#menu-toggle' ).on( 'click', function( e ) {
				e.preventDefault();
				$( '.page-site' ).css( {
					'-webkit-transform': 'translate3d( 320px, 0, 0 )',
					'-moz-transform': 'translate3d( 320px, 0, 0 )',
					'transform': 'translate3d( 320px, 0, 0 )'
				} );
				$( '.sidebar-nano' ).css( {
					'visibility': 'visible',
					'opacity': '1'
				} );
				$( 'body' ).addClass( 'sidebar-open' );
			} );
			$( '.show-submenu' ).each( function() {
				$( this ).on( 'click', function( e ) {
					e.preventDefault();
					$( this ).prev().slideUp( 300 );
					$( this ).children( 'i' ).removeClass( 'fa-chevron-circle-up' );
					$( this ).children( 'i' ).addClass( 'fa-chevron-circle-down' );
					if ( $( this ).prev().is( ':hidden' ) !== false ) {
						$( this ).prev().slideDown( 300 );
						$( this ).children( 'i' ).addClass( 'fa-chevron-circle-up' );
						$( this ).children( 'i' ).removeClass( 'fa-chevron-circle-down' );
					}
				} );
				$( 'body' ).removeClass( 'sidebar-open' );
			} );
			$( '.sidebar-menu-overlay' ).on( 'click', function( e ) {
				e.preventDefault();
				$( 'body' ).removeClass( 'sidebar-open' );
				$( '.page-site' ).css( {
					'-webkit-transform': 'translate3d( 0, 0, 0 )',
					'-moz-transform': 'translate3d( 0, 0, 0 )',
					'transform': 'translate3d( 0, 0, 0 )'
				} );
				$( '.sidebar-nano' ).css( {
					'visibility': 'hidden',
					'opacity': '0'
				} );
			} );

		} else if ( $( window ).width() < 600 ) {
			$( '<div class="show-submenu"><i class="fa fa-chevron-circle-down"></i></div>' ).appendTo( '.has-menu-item' );
			$( '<div class="sidebar-menu-overlay"></div>' ).appendTo( '.page-site' );
			$( '#menu-toggle' ).on( 'click', function( e ) {
				e.preventDefault();
				$( '.page-site' ).css( {
					'-webkit-transform': 'translate3d( 250px, 0, 0 )',
					'-moz-transform': 'translate3d( 250px, 0, 0 )',
					'transform': 'translate3d( 250px, 0, 0 )'
				} );
				$( '.sidebar-nano' ).css( {
					'visibility': 'visible',
					'opacity': '1'
				} );
				$( 'body' ).addClass( 'sidebar-open' );
			} );
			$( '.show-submenu' ).each( function() {
				$( this ).on( 'click', function( e ) {
					e.preventDefault();
					$( this ).prev().slideUp( 300 );
					$( this ).children( 'i' ).removeClass( 'fa-chevron-circle-up' );
					$( this ).children( 'i' ).addClass( 'fa-chevron-circle-down' );
					if ( $( this ).prev().is( ':hidden' ) !== false ) {
						$( this ).prev().slideDown( 300 );
						$( this ).children( 'i' ).addClass( 'fa-chevron-circle-up' );
						$( this ).children( 'i' ).removeClass( 'fa-chevron-circle-down' );
					}
				} );
				$( 'body' ).removeClass( 'sidebar-open' );
			} );
			$( '.sidebar-menu-overlay' ).on( 'click', function( e ) {
				e.preventDefault();
				$( 'body' ).removeClass( 'sidebar-open' );
				$( '.page-site' ).css( {
					'-webkit-transform': 'translate3d( 0, 0, 0 )',
					'-moz-transform': 'translate3d( 0, 0, 0 )',
					'transform': 'translate3d( 0, 0, 0 )'
				} );
				$( '.sidebar-nano' ).css( {
					'visibility': 'hidden',
					'opacity': '0'
				} );
			} );
		}
	}

	/**
	 * 7. Blog List Page
	 *
	 * @author	lox
	 */
	function homeline_blog_list_detail_page() {
		if ( !( $( 'body' ).hasClass( 'blog-list-page' ) ) ) return false;

		$( '.slider-range' ).slider( {
			range: true,
			min: 0,
			max: 15000,
			values: [ 2500, 8750 ],
			slide: function( event, ui ) {
				$( '.amount' ).val( '$' + ui.values[ 0 ] + ' - $' + ui.values[ 1 ] );
			}
		} );
		$( '.amount' ).val( '$' + $( '.slider-range' ).slider( 'values', 0 ) + ' - $' + $( '.slider-range' ).slider( 'values', 1 ) );
	}

	/**
	 * 8. Blog Grid Page
	 *
	 * @author	lox
	 */
	function homeline_blog_grid_detail_page() {
		if ( !( $( 'body' ).hasClass( 'blog-grid-page' ) || $( 'body' ).hasClass( 'blog-detail-page' ) ) ) return false;

		$( '.slider-range' ).slider( {
			range: true,
			min: 0,
			max: 15000,
			values: [ 2500, 8750 ],
			slide: function( event, ui ) {
				$( '.amount' ).val( '$' + ui.values[ 0 ] + ' - $' + ui.values[ 1 ] );
			}
		} );
		$( '.amount' ).val( '$' + $( '.slider-range' ).slider( 'values', 0 ) + ' - $' + $( '.slider-range' ).slider( 'values', 1 ) );
	}

	/**
	 * 9. Homepage
	 *
	 * @author	BaLqz
	 */
	function homeline_homepage_v1_page() {
		if ( !( $( 'body' ).hasClass( 'homepage' ) ) ) return false;

		$( '#accordion' ).accordion();
		$( '#tabs' ).tabs();

		$( '.counter-value' ).counterUp( {
			delay: 10,
			time: 1000
		} );

		$( '.slider-range' ).slider( {
			range: true,
			min: 0,
			max: 15000,
			values: [ 2500, 8750 ],
			slide: function( event, ui ) {
				$( '.amount' ).val( '$' + ui.values[ 0 ] + ' - $' + ui.values[ 1 ] );
			}
		} );
		$( '.amount' ).val( '$' + $( '.slider-range' ).slider( 'values', 0 ) + ' - $' + $( '.slider-range' ).slider( 'values', 1 ) );

		$( '.owl-carousel' ).owlCarousel( {
			loop: true,
			items: 1
		} );
	}

	/**
	 * 10. Homepage Map
	 *
	 * @author  lox
	 */
	function homeline_homepage_map() {
		if ( !( $( 'body' ).hasClass( 'homepage-v2' ) ) ) return false;

		var locations = [
				['Clarkson Ave', 40.656053, -73.939555, 5],
				['Bay Ridge', 40.626164, -74.032950, 4],
				['East New York', 40.656831, -73.883070, 3],
				['Foster Ave', 40.639292, -73.937278, 2],
				['Broad Channel', 40.615834, -73.821321, 1]
			],
			map = new google.maps.Map( document.getElementById( 'search-map' ), {
				center: 		{ lat: 40.643747, lng: -73.930104 },
				zoom: 			12,
				draggable: 		false,
				scrollwheel: 	false,
				styles: [
					{
						"elementType": "geometry",
						"stylers": [ {
							"color": "#f5f5f5"
						} ]
					},
					{
						"elementType": "labels.icon",
						"stylers": [ {
							"visibility": "off"
						} ]
					},
					{
						"elementType": "labels.text.fill",
						"stylers": [ {
							"color": "#616161"
						} ]
					},
					{
						"elementType": "labels.text.stroke",
						"stylers": [ {
							"color": "#f5f5f5"
						} ]
					},
					{
						"featureType": "administrative.land_parcel",
						"elementType": "labels.text.fill",
						"stylers": [ {
							"color": "#bdbdbd"
						} ]
					},
					{
						"featureType": "poi",
						"elementType": "geometry",
						"stylers": [ {
							"color": "#eeeeee"
						} ]
					},
					{
						"featureType": "poi",
						"elementType": "labels.text.fill",
						"stylers": [ {
							"color": "#757575"
						} ]
					},
					{
						"featureType": "poi.park",
						"elementType": "geometry",
						"stylers": [ {
							"color": "#e5e5e5"
						} ]
					},
					{
						"featureType": "poi.park",
						"elementType": "labels.text.fill",
						"stylers": [ {
							"color": "#9e9e9e"
						} ]
					},
					{
						"featureType": "road",
						"elementType": "geometry",
						"stylers": [ {
							"color": "#ffffff"
						} ]
					},
					{
						"featureType": "road.arterial",
						"elementType": "labels.text.fill",
						"stylers": [ {
							"color": "#757575"
						} ]
					},
					{
						"featureType": "road.highway",
						"elementType": "geometry",
						"stylers": [ {
							"color": "#dadada"
						} ]
					},
					{
						"featureType": "road.highway",
						"elementType": "labels.text.fill",
						"stylers": [ {
							"color": "#616161"
						} ]
					},
					{
						"featureType": "road.local",
						"elementType": "labels.text.fill",
						"stylers": [ {
							"color": "#9e9e9e"
						} ]
					},
					{
						"featureType": "transit.line",
						"elementType": "geometry",
						"stylers": [ {
							"color": "#e5e5e5"
						} ]
					},
					{
						"featureType": "transit.station",
						"elementType": "geometry",
						"stylers": [ {
							"color": "#eeeeee"
						} ]
					},
					{
						"featureType": "water",
						"elementType": "geometry",
						"stylers": [ {
							"color": "#c9c9c9"
						} ]
					},
					{
						"featureType": "water",
						"elementType": "labels.text.fill",
						"stylers": [ {
							"color": "#9e9e9e"
						} ]
					}
				]
			} ),
			infowindow = new google.maps.InfoWindow(),
			marker,
			i;

		for ( i = 0; i < locations.length; i++ ) {
			marker = new google.maps.Marker( {
				position: new google.maps.LatLng(locations[i][1], locations[i][2]),
				icon: './assets/img/map-marker.png',
				map: map
			} );

			google.maps.event.addListener( marker, 'click', ( function( marker, i ) {
				return function() {
					infowindow.setContent( locations[i][0] );
					infowindow.open( map, marker );
				}
			} )( marker, i ) );
		}
	}

	/**
	 * Fire all js function.
	 *
	 * @author lox
	 */
	function homeline_fire_js_function() {
		homeline_contact_us_page();
		homeline_about_us_page();
		homeline_property_listing_page();
		homeline_property_detail_page();
		homeline_blog_list_detail_page();
		homeline_blog_grid_detail_page();
		homeline_search_popup();
		homeline_scroll_header_mobile();
		homeline_homepage_v1_page();
		homeline_homepage_map();
	}
	homeline_fire_js_function();

	$( window ).resize( function() {
		homeline_fire_js_function();
	} );
} );