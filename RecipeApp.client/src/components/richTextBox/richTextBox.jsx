import React, { useState, useMemo, useCallback } from 'react'
import { createEditor, Editor } from 'slate'
import { Slate, Editable, withReact } from 'slate-react'

const CustomEditor = {
  isBoldMarkActive(editor) {
    const marks = Editor.marks(editor)
    return marks ? marks.bold === true : false
  },
  isItalicMarkActive(editor) {
    const marks = Editor.marks(editor)
    return marks ? marks.italic === true : false
  },
  isUnderlineMarkActive(editor) {
    const marks = Editor.marks(editor)
    return marks ? marks.underline === true : false
  },

  toggleBoldMark(editor) {
    const isActive = CustomEditor.isBoldMarkActive(editor)
    if (isActive) {
      Editor.removeMark(editor, 'bold')
    } else {
      Editor.addMark(editor, 'bold', true)
    }
  },
  toggleItalicMark(editor) {
    const isActive = CustomEditor.isItalicMarkActive(editor)
    if (isActive) {
      Editor.removeMark(editor, 'italic')
    } else {
      Editor.addMark(editor, 'italic', true)
    }
  },
  toggleUnderlineMark(editor) {
    const isActive = CustomEditor.isUnderlineMarkActive(editor)
    if (isActive) {
      Editor.removeMark(editor, 'underline')
    } else {
      Editor.addMark(editor, 'underline', true)
    }
  },
}

export const RichTextBox = () => {
  // Create a Slate editor object that won't change across renders
  const editor = useMemo(() => withReact(createEditor()), [])
  
  // Define the initial state with standard JS objects
  const [initialValue, setValue] = useState([
    {
      type: 'paragraph',
      children: [{ text: 'A line of text in a paragraph.' }],
    },
  ])

  const renderLeaf = useCallback(props => {
    return <Leaf {...props} />
  }, [])

  return (
    <Slate editor={editor} initialValue={initialValue} onChange={newValue => setValue(newValue)}>
      <Editable 
        renderLeaf={renderLeaf}
        onKeyDown={event => {
            if (!event.ctrlKey) {
                return
            }

            switch (event.key) {
                case 'b': {
                    event.preventDefault()
                    CustomEditor.toggleBoldMark(editor)
                    break
                }
                case 'i': {
                    event.preventDefault()
                    CustomEditor.toggleItalicMark(editor)
                    break
                }
                case 'u': {
                    event.preventDefault()
                    CustomEditor.toggleUnderlineMark(editor)
                    break
                }
            }
        }}
      />
    </Slate>
  )
}

const Leaf = props => {
    return (
        <span
            {...props.attributes}
            style={{ 
                fontWeight: props.leaf.bold ? 'bold' : 'normal',
                fontStyle: props.leaf.italic ? 'italic' : 'normal',
                textDecoration: props.leaf.underline ? 'underline' : 'none'
            }}
        >
            {props.children}
        </span>
    )
}

export default RichTextBox;