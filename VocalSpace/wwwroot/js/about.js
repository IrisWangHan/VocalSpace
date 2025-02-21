const layers = document.querySelectorAll('.about__layer');
const layer_spacing = -1200;
const z_values = [];

let prev_scroll_pos = layer_spacing / layers.length;
handlescroll();

function handlescroll() {
    const curr_scroll_pos = window.scrollY;
    const scroll_delta = prev_scroll_pos - curr_scroll_pos;
    prev_scroll_pos = curr_scroll_pos;

    // 取得設備的像素比率
    const devicePixelRatio = window.devicePixelRatio || 1;

    layers.forEach((layer, index) => {
        z_values.push(layer_spacing + (layer_spacing * index));
        // 根據裝置的像素比率調整滾動速度
        z_values[index] += (scroll_delta * -layers.length) * 1 * devicePixelRatio;

        let curr_z_value = z_values[index];
        layer.style.transform = `translateZ(${curr_z_value}px)`;

        console.log(curr_scroll_pos)

        let opacity = Math.min(1, Math.max(0, (1250 - Math.abs(curr_z_value)) / 1000));
        layer.querySelector(".about__content").style.opacity = opacity;

        let is_layer_hidden = curr_z_value > (layer_spacing) / -4;
        layer.classList.toggle('hide', is_layer_hidden);
    });
}

window.addEventListener('scroll', handlescroll);

//即使使用者重新整理，滾動條依然會回到頂部。
if ('scrollRestoration' in history) {
    history.scrollRestoration = 'manual';
}

window.onload = function () {
    window.scrollTo(0, 0);
};
