import React, { useState, useMemo, useCallback } from 'react'
import { createEditor, Editor } from 'slate'
import { Slate, Editable, withReact, ReactEditor } from 'slate-react'
import './richTextBox.css'

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
  isStrikethroughMarkActive(editor) {
    const marks = Editor.marks(editor)
    return marks ? marks.strikethrough === true : false
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
  toggleStrikethroughMark(editor) {
    const isActive = CustomEditor.isStrikethroughMarkActive(editor)
    if (isActive) {
      Editor.removeMark(editor, 'strikethrough')
    } else {
      Editor.addMark(editor, 'strikethrough', true)
    }
  }
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
    <Slate
      editor={editor}
      initialValue={initialValue}
      onChange={newValue => setValue(newValue)}>
      <div>
        <span
          role="button"
          style={{ cursor: 'pointer', padding: '5px', margin: '0 2px', display: 'inline-block', userSelect: 'none' }}
          onMouseDown={event => {
            event.preventDefault()
            CustomEditor.toggleBoldMark(editor)
          }}
        >
          <div>
            <b>B</b>
          </div>
        </span>
        <span
          role="button"
          style={{ cursor: 'pointer', padding: '5px', margin: '0 2px', display: 'inline-block', userSelect: 'none' }}
          onMouseDown={event => {
            event.preventDefault()
            CustomEditor.toggleItalicMark(editor)
          }}
        >
          <div>
            <i>I</i>
          </div>
        </span>
        <span
          role="button"
          style={{ cursor: 'pointer', padding: '5px', margin: '0 2px', display: 'inline-block', userSelect: 'none' }}
          onMouseDown={event => {
            event.preventDefault()
            CustomEditor.toggleUnderlineMark(editor)
          }}
        >
          <div>
            <u>U</u>
          </div>
        </span>
        <span
          role="button"
          style={{ cursor: 'pointer', padding: '5px', margin: '0 2px', display: 'inline-block', userSelect: 'none' }}
          onMouseDown={event => {
            event.preventDefault()
            CustomEditor.toggleStrikethroughMark(editor)
          }}
        >
          <div>
            <s>S</s>
          </div>
        </span>
      </div>
      <div className="richTextBox">
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
      </div>
    </Slate>
  )
}

const Leaf = props => {
  const decorations = [];
  if (props.leaf.underline) decorations.push('underline');
  if (props.leaf.strikethrough) decorations.push('line-through');

  return (
    <span
      {...props.attributes}
      style={{ 
        fontWeight: props.leaf.bold ? 'bold' : 'normal',
        fontStyle: props.leaf.italic ? 'italic' : 'normal',
        textDecoration: decorations.length ? decorations.join(' ') : 'none'
      }}
    >
      {props.children}
    </span>
  )
}

export default RichTextBox;