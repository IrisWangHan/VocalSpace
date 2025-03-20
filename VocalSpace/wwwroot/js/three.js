var earthRenderer, earthCamera, earthScene, earthObject;
var earthWidth, earthHeight;

/* 【    地球    】*/
// 初始化地球渲染器
function initEarthRenderer() {
    earthWidth = document.getElementById('banner_earth').clientWidth;
    earthHeight = document.getElementById('banner_earth').clientHeight;
    earthRenderer = new THREE.WebGLRenderer({
        canvas: document.getElementById('banner_earth'),
        alpha: true
    });
    earthRenderer.setClearColor(0x000000,0);
    earthRenderer.setSize(earthWidth, earthHeight);
}

// 初始化地球相機
function initEarthCamera() {
    earthCamera = new THREE.PerspectiveCamera(75, earthWidth / earthHeight, 0.1, 1000);
    earthCamera.position.set(50, 50, 50); //x,y,z(數值越小越近)
    earthCamera.lookAt(new THREE.Vector3(50, 50, 50)); // 相機針對場景中心
    earthScene = new THREE.Scene(); // 初始化場景
}

// 初始化地球物件
function initEarth() {
    var geometry = new THREE.SphereGeometry(50, 32, 32); // 建立球體幾何形狀

    var textureLoader = new THREE.TextureLoader();
    textureLoader.load('/image/earth3.jpeg', function (texture) {
        texture.wrapS = THREE.MirroredRepeatWrapping;
        texture.wrapT = THREE.RepeatWrapping;  // 設置垂直重複
        texture.repeat.x = 2;  // 水平方向重複兩次
        texture.repeat.y = 2;  // 垂直方向重複兩次
        var material = new THREE.MeshBasicMaterial({
            map: texture, // 應用紋理
            shininess: 10,  // 控制表面光澤
        });



        earthObject = new THREE.Mesh(geometry, material);
        earthScene.add(earthObject); // 加入地球物件

        renderEarth(); // 確保在紋理加載後才開始渲染
    });
}

// 渲染並更新位置
function renderEarth() {
    requestAnimationFrame(renderEarth);

    if (earthObject) { // 確保地球物件存在
        earthObject.rotation.x-= 0.001;
        earthObject.rotation.y +=0.002; // 讓地球向右旋轉 90 度

    }

    earthRenderer.render(earthScene, earthCamera);
}

// 初始化所有內容
function init() {
    initEarthRenderer();
    initEarthCamera();
    initEarth();
}


window.onload = init;




