import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import { CKEditor } from "@ckeditor/ckeditor5-react";

interface IEditorProps {
    data: string;
    setData: (text: string) => void;
}

function Editor({ data, setData }: IEditorProps) {
    return (
        <div className="editor">
            <CKEditor
                data={data}
                editor={ClassicEditor}
                config={{
                    toolbar: ['heading', '|', 'bold', 'italic', 'link', 'bulletedList', 'numberedList', 'blockQuote'],
                    heading: {
                        options: [
                            { model: 'paragraph', title: 'Paragraph', class: 'ck-heading_paragraph' },
                            { model: 'heading1', view: 'h1', title: 'Heading 1', class: 'ck-heading_heading1' },
                            { model: 'heading2', view: 'h2', title: 'Heading 2', class: 'ck-heading_heading2' }
                        ]
                    },
                    placeholder: "Введите текст здесь..."
                }}
                onChange={(event, editor) => {
                    setData(editor.getData());
                }}
            />
        </div>

    );
}

export default Editor;