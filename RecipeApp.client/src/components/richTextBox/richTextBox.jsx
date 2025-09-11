import { useState } from 'react';
import { createEditor } from 'slate';
import { Slate, Editable, withReact } from 'slate-react'
import './richTextBox.css';

const initialValue = [
    {
        type: 'paragraph',
        children: [{ text: '' }],
    },
]

function RichTextBox() {
    const [editor] = useState(() => withReact(createEditor()))

    return (
        <>
            <Slate editor={editor} initialValue={initialValue} >
                <Editable className="richTextBox_textbox" />
            </Slate>
        </>
    );
}

export default RichTextBox;