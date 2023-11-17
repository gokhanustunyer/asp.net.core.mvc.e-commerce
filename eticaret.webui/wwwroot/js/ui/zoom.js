(function () {

	if (typeof $ !== "function")
		throw Error('JQuery is not present.');

	var times = 2, handler;

	var init = function () {

		var t = $(this),
			p = t.parent(),
			v = p.next(),
			cs = v.next(),
			iw = v.children();

		handler = function (e) {

			var [w, h] = ['width', 'height'].map(x => $.fn[x].call(t)),
				nw = w * times, nh = h * times, cw = w / times, ch = h / times;

			var eventMap = {
				mousemove: function (e) {

					e = e.originalEvent;

					var x = e.layerX,
						y = e.layerY,
						rx = cw / 2,
						ry = ch / 2,
						cx = x - rx,
						cy = y - ry,
						canY = cy >= 0 && cy <= h - ch,
						canX = cx >= 0 && cx <= w - cw

					cs.css({
						top: canY ? cy : cy < 0 ? 0 : h - ch,
						left: canX ? cx : cx < 0 ? 0 : w - cw
					});

					iw.css({
						top: canY ? -cy / (h - ch) * (nh - h) : cy < 0 ? 0 : -(nh - h),
						left: canX ? -cx / (w - cw) * (nw - w) : cx < 0 ? 0 : -(nw - w)
					});
				}
			};

			p.width(w).height(h);
			cs.width(cw).height(ch);
			iw.width(nw).height(nh);

			for (let k in eventMap)
				p.on(k, eventMap[k]);
		};

		t.on('load', handler);
	};

	$.fn.extend({

		zoom: function (t) {
			times = t || times;

			for (let x of this)
				init.call(x);

			return this;
		},
		setZoom: function (t) {

			times = t || times;

			if (handler === void 0)
				throw Error('Zoom not initialized.');

			handler();

		}

	});

}());

//var changeImage = (obj) => {
//	const zoom = document.querySelector('.main-image');
//	let original = zoom.querySelector('img');
//	//let viewer = zoom.querySelector('.viewer').querySelector('img');
//	original.src = obj.src;
//	//viewer.src = obj.src;
//}

window.addEventListener('load', () => {
	var h = document.querySelector('.zoom').offsetHeight;
	document.querySelector('.thumbnails').setAttribute('style', `height: ${h}px; width:100px`);
});
