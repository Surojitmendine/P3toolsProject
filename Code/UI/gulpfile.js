const { src, dest, parallel, watch, series, task, gulp } = require('gulp');
let cleanCSS = require('gulp-clean-css');
var concat = require('gulp-concat');
const terser = require('gulp-terser');
let babel = require('gulp-babel');
const del = require('del');
var replace = require('gulp-replace');
var debug = require('gulp-debug');
var browsersync = require("browser-sync").create();
const htmlmin = require('gulp-htmlmin');
var log = require('fancy-log');
var inject = require('gulp-inject');
const javascriptObfuscator = require('gulp-javascript-obfuscator');

var gulpif = require('gulp-if');
const commander = require('commander');
const program = new commander.Command();

program
    .option('-loc, --local', 'output extra debugging')
    .option('-prod, --production', 'output extra debugging');

program.parse(process.argv)


function clean() {
    return del('publish/**', { force: true });
}

var fileslibraryCSS = ['plugins/fontawesome-free/css/all.css', 'plugins/bootstrap-431/css/bootstrap.min.css',
    'plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css',
    'plugins/icheck-bootstrap/icheck-bootstrap.min.css', 'plugins/jqvmap/jqvmap.min.css', 'app/dist/css/adminlte.min.css',
    'plugins/overlayScrollbars/css/OverlayScrollbars.min.css', 'plugins/daterangepicker/daterangepicker.css',
    'plugins/summernote/summernote-bs4.css', 'plugins/sweetalert2-theme-bootstrap-4/bootstrap-4.min.css',
    'plugins/faLoading/jquery.faloading.css', 'plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css',
    'plugins/select2/css/select2.min.css', 'plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css',
    'plugins/icheck-bootstrap/icheck-bootstrap.min.css', 'plugins/nprogress/nprogress.css', 'plugins/summernote/summernote-bs4.css',
    'plugins/PrintJs/print.min.css']

var filesLibraryCSSFont = ['plugins/fontawesome-free/webfonts/*', 'plugins/summernote/font/*']

var filesTemplateCustomCSS = ['app/dist/css/adminlte.css', 'app/dist/css/framework-customization.css']

var filePrintCSS = ['app/dist/css/print.css']

var filesLibraryJS = ['plugins/jquery/jquery.min.js', 'plugins/jquery-ui/jquery-ui.min.js', 'plugins/bootstrap/js/bootstrap.bundle.min.js',
    'plugins/chart.js/Chart.min.js', 'plugins/sparklines/sparkline.js', 'plugins/jquery-knob/jquery.knob.min.js', 'plugins/moment/moment.min.js',
    'plugins/daterangepicker/daterangepicker.js', 'plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js',
    'plugins/summernote/summernote-bs4.min.js', 'plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js',
    'plugins/datatables/jquery.dataTables.js', 'plugins/datatables-bs4/js/dataTables.bootstrap4.js', 'plugins/datatables-bs4/js/buttons/dataTables.buttons.min.js',
    'plugins/datatables-bs4/js/buttons/buttons.flash.min.js', 'plugins/datatables-bs4/js/buttons/jszip.min.js', 'plugins/datatables-bs4/js/buttons/buttons.html5.min.js',
    'plugins/datatables-bs4/js/buttons/buttons.print.min.js', 'plugins/datatables-bs4/js/buttons/datetime-moment.js',
    'plugins/sweetalert2/sweetalert2.min.js', 'plugins/faLoading/jquery.faloading.js', 'plugins/bootstrap-datetimepicker/js/moment.min.js',
    'plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js', 'plugins/select2/js/select2.full.min.js', 'plugins/nprogress/nprogress.js',
    'plugins/summernote/summernote-bs4.min.js', 'plugins/chart.js/Chart.min.js', 'plugins/PrintJs/print.min.js',
    'plugins/pagejs/page.js','plugins/bootstrap-switch/js/bootstrap-switch.min.js','plugins/bootbox/bootbox.min.js','plugins/tabletoexcel/tabletoexcel.js']

var filesTemplateCustomJS = ['app/dist/js/adminlte.js', 'app/dist/js/demo.js', 'app/scripts/config.js', 'app/scripts/00.global.js']

var filesFrameworkGlobalJS = ['app/scripts/config.js','app/scripts/01.global-router.js', 'app/scripts/00.global.js']

var filesFrameworkScripts = ['app/scripts/AmountInWord.js', 'app/scripts/api-call.js', 'app/scripts/common.js',
    'app/scripts/dashboard.js', 'app/scripts/**/*.js', '!00.global.js', '!config.js']

var filesStaticAsets = ['app/dist/img/*']

var filePage = ['app/index.html', 'app/login.html', 'app/favicon.ico']

var filePages = ['app/pages/**/*']

function libraryCSS() {
    return src(fileslibraryCSS)
        .pipe((gulpif(program.production,cleanCSS())))
        .pipe(debug({
            logger: function (message) {
                log(message)
            }

        }))
        .pipe(concat('library.style.min.css'))
        .pipe(replace('../webfonts', './font'))
        // .pipe(replace('./font/summernote.eot?1d9aeaaff0a8939558a45be6cd52cd4c', './webfonts/summernote.eot?1d9aeaaff0a8939558a45be6cd52cd4c'))
        // .pipe(replace('./font/summernote.eot?1d9aeaaff0a8939558a45be6cd52cd4c#iefix', './webfonts/summernote.eot?1d9aeaaff0a8939558a45be6cd52cd4c#iefix'))
        // .pipe(replace('./font/summernote.woff?1d9aeaaff0a8939558a45be6cd52cd4c', './webfonts/summernote.woff?1d9aeaaff0a8939558a45be6cd52cd4c'))
        // .pipe(replace('./font/summernote.ttf?1d9aeaaff0a8939558a45be6cd52cd4c', './webfonts/summernote.ttf?1d9aeaaff0a8939558a45be6cd52cd4c'))
        .pipe(dest('publish'))
    //.pipe(browsersync.stream());

}

function libraryCSSFont() {
    return (
        src(filesLibraryCSSFont)
            .pipe(debug({
                logger: function (message) {
                    log(message)
                }

            }))
            .pipe(dest('publish/font'))
        // .pipe(browsersync.stream())
    );
}

function templateCustomCSS() {
    return (
        src(filesTemplateCustomCSS)
        .pipe((gulpif(program.production,cleanCSS())))
            .pipe(debug({
                logger: function (message) {
                    log(message)
                }

            }))
            .pipe(concat('template-custom.style.min.css'))
            .pipe(dest('publish'))
        //.pipe(browsersync.stream())
    );
}

function CopyPrintCss() {
    return (
        src(filePrintCSS)
        .pipe((gulpif(program.production,cleanCSS())))
            .pipe(debug({
                logger: function (message) {
                    log(message)
                }

            }))
            .pipe(dest('publish/dist'))
        //.pipe(browsersync.stream())
    );
}

function libraryJS() {
    return (
        src(filesLibraryJS)
            .pipe(concat('plaugin-library.js'))
            //.pipe(uglify())
            .pipe(debug({
                logger: function (message) {
                    log(message)
                }

            }))
            .pipe((gulpif(program.production,terser())))
            .pipe(dest('publish'))
        //.pipe(browsersync.stream())
    );
}

function templateCustomJS() {
    return (
        src(filesTemplateCustomJS)
            .pipe(concat('template-custom-js.js'))
            //.pipe(uglify())
            .pipe(debug({
                logger: function (message) {
                    log(message)
                }

            }))
            .pipe((gulpif(program.production,terser())))
            .pipe(dest('publish'))
        //.pipe(browsersync.stream())
    );
}


function frameworkGlobalJS() {
    return (
        src(filesFrameworkGlobalJS)
            .pipe(concat('framework-global-js.js'))
            //.pipe(javascriptObfuscator())
            //.pipe(uglify())
            .pipe(debug({
                logger: function (message) {
                    log(message)
                }

            }))
            .pipe(gulpif(program.production,terser()))
            .pipe(gulpif(program.production, javascriptObfuscator({
                compact: true,
                stringArray: true,
                stringArrayEncoding: true,
                stringArrayThreshold: 0.75,

            })))
            .pipe(dest('publish'))
        //.pipe(browsersync.stream())
    );
}
function frameworkScripts() {
    return src(filesFrameworkScripts)
        .pipe(babel({
            presets: ['@babel/env'],
        }))

        .pipe(concat('framework-scripts.js'))
        .pipe(debug({
            logger: function (message) {
                log(message)
            }

        }))
        .pipe(gulpif(program.production,terser()))
        .pipe(gulpif(program.production, javascriptObfuscator({
            compact: true,
            stringArray: true,
            stringArrayEncoding: true,
            stringArrayThreshold: 0.75,

        })))
        .pipe(dest('publish'))
    // .pipe(browsersync.stream())
}

function copyStaticAsets() {
    return (
        src(filesStaticAsets)
            .pipe(debug({
                logger: function (message) {
                    log(message)
                }

            }))
            .pipe(dest('publish/dist/img'))
        //.pipe(browsersync.stream())
    );
}



function copyPage() {
    return (
        src(filePage)
            .pipe(debug({
                logger: function (message) {
                    log(message)
                }
            }))
            .pipe(gulpif(program.production,htmlmin({ collapseWhitespace: true })))
            .pipe(dest('publish'))
        //.pipe(browsersync.stream())
    );
}

function copyPages() {
    return (
        src(filePages)
            .pipe(debug({
                logger: function (message) {
                    log(message)
                }

            }))
            .pipe(gulpif(program.production,htmlmin({ collapseWhitespace: true })))
            .pipe(dest('publish/pages'))
        //.pipe(browsersync.stream())
    );
}



// function inject() {
//     var target = src('./publish/index.html');
//     var sources = src('./publish/*.js');

//     return target.pipe(inject(sources))
//     .pipe(dest('./publish'));

// };

watch('gulpfile.js', series(clean, parallel(libraryCSS, libraryCSSFont, templateCustomCSS,
    libraryJS, templateCustomJS, frameworkGlobalJS, frameworkScripts,
    copyStaticAsets, copyPage, copyPages), browserSyncReload));

watch(filesTemplateCustomCSS, series(parallel(templateCustomCSS), browserSyncReload));

watch(filesTemplateCustomJS, series(parallel(templateCustomJS), browserSyncReload));

watch(filesFrameworkGlobalJS, series(parallel(frameworkGlobalJS), browserSyncReload));

watch(filesFrameworkScripts, series(parallel(frameworkScripts), browserSyncReload));

watch(filesStaticAsets, series(parallel(copyStaticAsets), browserSyncReload));

watch(filePage, series(parallel(copyPage), browserSyncReload));

watch(filePages, series(parallel(copyPages), browserSyncReload));

watch(filePrintCSS, series(parallel(CopyPrintCss), browserSyncReload));

// BrowserSync
function browserSync() {
    browsersync.init({
        server: {
            baseDir: "./publish/",
        },
        port: 3000,
        browser: "chrome.exe",
        logLevel: "debug",
        logPrefix: "P3Tools ==>",
        logFileChanges: true
    });
}
function browserSyncReload(done) {
    browsersync.reload();
    done();
}

exports.clean = clean;
exports.css = parallel(libraryCSS, libraryCSSFont, templateCustomCSS);
exports.js = parallel(libraryJS, templateCustomJS, frameworkGlobalJS, frameworkScripts);
exports.copy = parallel(copyStaticAsets, copyPage, copyPages, CopyPrintCss)
exports.inject = inject
exports.publish = series(clean, parallel(this.css, this.js, this.copy)/*,this.inject*/, browserSync)

exports.default = parallel(this.publish);

