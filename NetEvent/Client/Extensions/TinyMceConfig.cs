using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace NetEvent.Client.Extensions
{
    [ExcludeFromCodeCoverage(Justification = "Ignore UI Config")]
    public static class TinyMceConfig
    {
        private const int _HeaderHeight = 64;
        private const int _EditorHeight = 500;

        public const string ScriptSrc = "/lib/tinymce/tinymce.min.js";

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
            { "toolbar_sticky_offset", _HeaderHeight },
            { "height", _EditorHeight },
            { "quickbars_insert_toolbar", "quicktable image emoticons quicklink hr | styles | fontfamily fontsize forecolor" },
            { "quickbars_selection_toolbar", "bold italic underline | styles | fontfamily fontsize forecolor | blocks | bullist numlist | indent outdent | blockquote quicklink" },
        };
    }
}
