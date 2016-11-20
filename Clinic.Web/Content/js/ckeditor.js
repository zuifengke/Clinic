
var editor = null;
function initeditor() {
    CKEDITOR.plugins.addExternal('flash', '/plugins/flash/', 'plugin.js');
    CKEDITOR.plugins.addExternal('colorbutton', '/plugins/colorbutton/', 'plugin.js');
    CKEDITOR.plugins.addExternal('justify', '/plugins/justify/', 'plugin.js');
    CKEDITOR.plugins.addExternal('embed', '/plugins/embed/', 'plugin.js');

    editor = CKEDITOR.replace('Content', {
        toolbar: 'Standard',
        height: 350,
        extraPlugins: 'flash,colorbutton,justify,embed'
    });
}