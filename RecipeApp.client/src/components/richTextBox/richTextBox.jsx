import React, { useState, useMemo, useCallback } from 'react'
import { createEditor, Editor, Transforms } from 'slate'
import { Slate, Editable, withReact, ReactEditor } from 'slate-react'
import './richTextBox.css'
import { BoldIcon } from '../tiptap-icons/bold-icon'
import { ItalicIcon } from '../tiptap-icons/italic-icon'
import { UnderlineIcon } from '../tiptap-icons/underline-icon'
import { StrikeIcon } from '../tiptap-icons/strike-icon'
import { ListIcon } from '../tiptap-icons/list-icon'
import { ListOrderedIcon } from '../tiptap-icons/list-ordered-icon'
import { HeadingIcon } from '../tiptap-icons/heading-icon'
import { HeadingTwoIcon } from '../tiptap-icons/heading-two-icon'

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
  },

  isListActive(editor) {
    const [match] = Editor.nodes(editor, {
      match: n => n.type === 'bulleted-list',
    })
    return !!match
  },

  isNumberedListActive(editor) {
    const [match] = Editor.nodes(editor, {
      match: n => n.type === 'numbered-list',
    })
    return !!match
  },

  toggleBulletList(editor) {
    const isActive = CustomEditor.isListActive(editor)

    Transforms.unwrapNodes(editor, {
      match: n => n.type === 'bulleted-list',
      split: true,
    })

    Transforms.setNodes(editor, {
      type: isActive ? 'paragraph' : 'list-item',
    })

    if (!isActive) {
      const block = { type: 'bulleted-list', children: [] }
      Transforms.wrapNodes(editor, block)
    }
  },

  toggleNumberedList(editor) {
    const isActive = CustomEditor.isNumberedListActive(editor)

    // Clean up both list types before toggling
    Transforms.unwrapNodes(editor, {
      match: n => n.type === 'bulleted-list' || n.type === 'numbered-list',
      split: true,
    })

    Transforms.setNodes(editor, {
      type: isActive ? 'paragraph' : 'list-item',
    })

    if (!isActive) {
      const block = { type: 'numbered-list', children: [] }
      Transforms.wrapNodes(editor, block)
    }
  }
}

export const RichTextBox = ({ value, onChange, name}) => {
  const editor = useMemo(() => withReact(createEditor()), [])

  const emptyValue = [
    {
      type: 'paragraph',
      children: [{ text: '' }],
    },
  ]
  
  const [editorValue, setEditorValue] = useState(
    value && value.length > 0 ? value : emptyValue
  )

  const renderLeaf = useCallback(props => {
    return <Leaf {...props} />
  }, [])

  const renderElement = useCallback(props => {
    switch (props.element.type) {
      case 'bulleted-list':
        return <ul {...props.attributes}>{props.children}</ul>
      case 'numbered-list':
        return <ol {...props.attributes}>{props.children}</ol>
      case 'list-item':
        return <li {...props.attributes}>{props.children}</li>
      default:
        return <p {...props.attributes}>{props.children}</p>
    }
  }, [])

  return (
    <Slate
      editor={editor}
      initialValue={emptyValue}
      value={editorValue}
      onChange={newValue => {
        setEditorValue(newValue)
        onChange({
          target: {
            name: name,
            value: newValue
          }
        })
      }}>
      <div>
        <div className="toolbar">
          <span
            role="button"
            onMouseDown={event => {
              event.preventDefault()
              CustomEditor.toggleBoldMark(editor)
            }}
          >
            <BoldIcon />
          </span>
        </div>
          <div className="toolbar">
          <span
            role="button"
            onMouseDown={event => {
              event.preventDefault()
              CustomEditor.toggleItalicMark(editor)
            }}
          >
            <ItalicIcon />
          </span>
        </div>
        <div className="toolbar">
          <span
            role="button"
            onMouseDown={event => {
              event.preventDefault()
              CustomEditor.toggleUnderlineMark(editor)
            }}
          >
            <div>
              <UnderlineIcon />
            </div>
          </span>
        </div>
        <div className="toolbar">
          <span
            role="button"
            onMouseDown={event => {
              event.preventDefault()
              CustomEditor.toggleStrikethroughMark(editor)
            }}
          >
            <div>
              <StrikeIcon />
            </div>
          </span>
        </div>
        <div className="toolbar">
          <span
            role="button"
            onMouseDown={event => {
              event.preventDefault()
              CustomEditor.toggleBulletList(editor)
            }}
          >
            <div>
              <ListIcon />
            </div>
          </span>
        </div>
        <div className="toolbar">
          <span
            role="button"
            onMouseDown={event => {
              event.preventDefault()
              CustomEditor.toggleNumberedList(editor)
            }}
          >
            <div>
              <ListOrderedIcon />
            </div>
          </span>
        </div>
        <div className="toolbar">
          <span
            role="button"
            onMouseDown={event => {
              event.preventDefault()
              CustomEditor.toggleNumberedList(editor)
            }}
          >
            <div>
              <HeadingIcon />
            </div>
          </span>
        </div>
        <div className="toolbar">
          <span
            role="button"
            onMouseDown={event => {
              event.preventDefault()
              CustomEditor.toggleNumberedList(editor)
            }}
          >
            <div>
              <HeadingTwoIcon />
            </div>
          </span>
        </div>
      </div>
      <div className="richTextBox">
        <Editable 
          renderLeaf={renderLeaf}
          renderElement={renderElement}
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

  renderNode = (props, editor, next) => {
    switch (props.node.type) {
      case 'paragraph':
        return (
          <p {...props.attributes} className={props.node.data.get('className')}>
            {props.children}
          </p>
        )
      default:
        return next()
    }
  }

  renderMark = (props, editor, next) => {
    const { mark, attributes } = props
    switch (mark.type) {
      case 'bold':
        return <strong {...attributes}>{props.children}</strong>
      case 'italic':
        return <em {...attributes}>{props.children}</em>
      case 'underline':
        return <u {...attributes}>{props.children}</u>
      default:
        return next()
    }
  }
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