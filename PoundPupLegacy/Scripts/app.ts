import ClassicEditorBase from '@ckeditor/ckeditor5-editor-classic/src/classiceditor';
import EssentialsPlugin from '@ckeditor/ckeditor5-essentials/src/essentials';
import UploadAdapterPlugin from '@ckeditor/ckeditor5-adapter-ckfinder/src/uploadadapter';
import AutoformatPlugin from '@ckeditor/ckeditor5-autoformat/src/autoformat';
import BoldPlugin from '@ckeditor/ckeditor5-basic-styles/src/bold';
import ItalicPlugin from '@ckeditor/ckeditor5-basic-styles/src/italic';
import BlockQuotePlugin from '@ckeditor/ckeditor5-block-quote/src/blockquote';
import HeadingPlugin from '@ckeditor/ckeditor5-heading/src/heading';
import ImagePlugin from '@ckeditor/ckeditor5-image/src/image';
import ImageCaptionPlugin from '@ckeditor/ckeditor5-image/src/imagecaption';
import ImageStylePlugin from '@ckeditor/ckeditor5-image/src/imagestyle';
import ImageToolbarPlugin from '@ckeditor/ckeditor5-image/src/imagetoolbar';
import ImageUploadPlugin from '@ckeditor/ckeditor5-image/src/imageupload';
import LinkPlugin from '@ckeditor/ckeditor5-link/src/link';
import ListPlugin from '@ckeditor/ckeditor5-list/src/list';
import ParagraphPlugin from '@ckeditor/ckeditor5-paragraph/src/paragraph';
import SourceEditing from '@ckeditor/ckeditor5-source-editing/src/sourceediting';
import SimpleUploadAdapter from '@ckeditor/ckeditor5-upload/src/adapters/simpleuploadadapter';

const editors = {};
export function setupEditor(selector: string, host: string, dotNetReference) {
    ClassicEditorBase
        .create(document.querySelector(selector), {
            plugins: [
                SourceEditing,
                EssentialsPlugin,
                UploadAdapterPlugin,
                AutoformatPlugin,
                BoldPlugin,
                ItalicPlugin,
                BlockQuotePlugin,
                HeadingPlugin,
                ImagePlugin,
                ImageCaptionPlugin,
                ImageStylePlugin,
                ImageToolbarPlugin,
                ImageUploadPlugin,
                LinkPlugin,
                ListPlugin,
                ParagraphPlugin,
                SimpleUploadAdapter],
            toolbar: {
                items: [
                    'heading',
                    '|',
                    'bold',
                    'italic',
                    'link',
                    'bulletedList',
                    'numberedList',
                    'uploadImage',
                    'blockQuote',
                    'undo',
                    'redo',
                    'sourceEditing',
                ]
            },
            image: {
                toolbar: [
                    'imageStyle:inline',
                    'imageStyle:block',
                    'imageStyle:side',
                    '|',
                    'toggleImageCaption',
                    'imageTextAlternative'
                ]
            },
            language: 'en',
            simpleUpload: {
            // The URL that the images are uploaded to.
                uploadUrl: 'https://'+host+'/image/upload',

                // Enable the XMLHttpRequest.withCredentials property.
                withCredentials: true,

                // Headers sent along with the XMLHttpRequest to the upload server.
                headers: {
                    'X-CSRF-TOKEN': 'CSRF-Token',
                    Authorization: 'Bearer <JSON Web Token>'
                }
        }
    } )
        .then(editor => {
            editors[selector] = editor;
            editor.model.document.on('change:data', () => {
                let data = editor.getData();

                const el = document.createElement('div');
                el.innerHTML = data;
                if (el.innerText.trim() == '')
                    data = null;

                dotNetReference.invokeMethodAsync('EditorDataChanged', data);
            });
        })
        .catch(error => console.error(error));
}

