import { TextStyle } from '@tiptap/extension-text-style'
import { EditorContent, useEditor } from '@tiptap/react'
import StarterKit from '@tiptap/starter-kit'
import React from 'react'
import { MenuBar } from './MenuBar.jsx'

import './richTextBox.css'

const extensions = [TextStyle, StarterKit]

export default ({ value, onChange, name = 'description' }) => {
  const onChangeRef = React.useRef(onChange)
  React.useEffect(() => {
    onChangeRef.current = onChange
  }, [onChange])

  const initialContent = React.useMemo(() => {
    if (typeof value === 'string' && value.length > 0) {
      return value
    }
    return ''
  }, [])

  const editor = useEditor({
    extensions,
    content: initialContent,
  
    onUpdate: ({ editor }) => {
      const htmlContent = editor.getHTML()
      const finalPayload = editor.isEmpty ? '' : htmlContent

      if (onChangeRef.current) {
        onChangeRef.current({
          target: {
            name: name,
            value: finalPayload
          }
        })
      }
    }
  })

  return (
    <>
      <MenuBar editor={editor} />
      <EditorContent editor={editor} />
    </>
  )
}
