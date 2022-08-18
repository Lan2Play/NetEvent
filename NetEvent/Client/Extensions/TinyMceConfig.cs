using System.Collections.Generic;

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
            { "plugins", "image code link lists advlist preview table visualchars wordcount fullscreen emoticons autoresize" },
            { "branding", false },
            { "automatic_uploads", true },
            { "images_upload_url", "/api/system/editorimage" },
            { "images_reuse_filename", true },
            { "image_title", true },
            { "image_advtab", true },
            { "link_context_toolbar", true },
            { "fullscreen_native", true },
            { "file_picker_types", "image" },
            { "toolbar_mode", "sliding" },
            { "toolbar_sticky", "true" },
            { "toolbar", "undo redo | styles | bold italic | alignleft aligncenter alignright alignjustify | indent outdent | numlist bullist | link image code table emoticons | visualchars preview fullscreen" }
        };
    }
}
