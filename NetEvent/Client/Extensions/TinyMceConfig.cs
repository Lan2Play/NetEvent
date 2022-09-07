using System.Collections.Generic;
using System.Globalization;

namespace NetEvent.Client.Extensions
{
    public class TinyMceConfig
    {
        /// <summary>
        /// Provide Editor Config for TinyMce
        /// More Information: https://www.tiny.cloud/docs/tinymce/6/editor-important-options/
        /// </summary>
        public static Dictionary<string, object> EditorConf { get; } = new Dictionary<string, object>
        {
            { "inline", true },
            { "menubar", false },
            { "event_root", "#root" },
            { "toolbar", false },
            { "skin", "borderless" },
            { "plugins", "image code link lists advlist preview table visualchars wordcount fullscreen emoticons quickbars" },
            { "branding", false },
            { "automatic_uploads", true },
            { "images_upload_url", "/api/system/editorimage" },
            { "images_reuse_filename", true },
            { "image_title", true },
            { "image_advtab", true },
            { "link_context_toolbar", true },
            { "fullscreen_native", true },
            { "file_picker_types", "image" },
            { "language", CultureInfo.CurrentUICulture.TwoLetterISOLanguageName },
            { "toolbar_mode", "sliding" },
            { "toolbar_sticky", true },
            { "toolbar_sticky_offset", 64 },
            { "height", 500 },
            { "quickbars_insert_toolbar", "quicktable image emoticons codesample quicklink | hr pagebreak" },
            { "quickbars_selection_toolbar", "bold italic underline | blocks | bullist numlist | indent outdent | blockquote quicklink" },
            //{ "contextmenu", "undo redo removeformat | inserttable | cell row column deletetable | code preview fullscreen | help" },
            //{ "toolbar", "undo redo removeformat | styles | fontfamily fontsize forecolor | bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | indent outdent | numlist bullist | link image table emoticons | code preview fullscreen" }
        };
    }
}
