import { useEditor, EditorContent, EditorContext } from '@tiptap/react'
import { useMemo } from 'react'
import { SimpleEditor } from '@/components/tiptap-templates/simple/simple-editor'
import Document from '@tiptap/extension-document'
import Paragraph from '@tiptap/extension-paragraph'
import Text from '@tiptap/extension-text'
import './richTextBox.css'

export const RichTextBox = ({ value, onChange, name }) => {
  const editor = useEditor({
    extensions: [SimpleEditor, Document, Paragraph, Text],
    content: value,
  })

  const providerValue = useMemo(() => ({ editor }), [editor])

  return (
    <SimpleEditor value={providerValue} />
    // <EditorContext.Provider value={providerValue}>
    //   <EditorContent editor={editor} value={editor.content} />
    //   {/* <FloatingMenu editor={editor}>This is the floating menu</FloatingMenu>
    //   <BubbleMenu editor={editor}>This is the bubble menu</BubbleMenu> */}
    // </EditorContext.Provider>
  )
}

export default RichTextBox;