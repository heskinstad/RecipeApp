import { useEditorState } from '@tiptap/react'
import React from 'react'

import { menuBarStateSelector } from './menuBarState.jsx'
import './menuBar.css'
import { BoldIcon } from '../tiptap-icons/bold-icon.jsx'
import { ItalicIcon } from '../tiptap-icons/italic-icon.jsx'
import { UnderlineIcon } from '../tiptap-icons/underline-icon.jsx'
import { StrikeIcon } from '../tiptap-icons/strike-icon.jsx'
import { ListIcon } from '../tiptap-icons/list-icon.jsx'
import { ListOrderedIcon } from '../tiptap-icons/list-ordered-icon.jsx'
import { HeadingOneIcon } from '../tiptap-icons/heading-one-icon.jsx'
import { HeadingTwoIcon } from '../tiptap-icons/heading-two-icon.jsx'
import { Undo2Icon } from '../tiptap-icons/undo2-icon.jsx'
import { Redo2Icon } from '../tiptap-icons/redo2-icon.jsx'

export const MenuBar = ({ editor }) => {
  const editorState = useEditorState({
    editor,
    selector: menuBarStateSelector,
  })

  if (!editor) {
    return null
  }

  return (
    <div className="control-group">
      <div className="button-group">
        <button // Need this functionless button or else bold formatting will be selected
          type="button"
          onClick={() => editor.chain().focus()}
          className="hidden-button" />
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleBold().run()}
          className={editorState.isBold ? 'is-active' : ''}
        >
          <BoldIcon />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleItalic().run()}
          className={editorState.isItalic ? 'is-active' : ''}
        >
          <ItalicIcon />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleUnderline().run()}
          className={editorState.isUnderline ? 'is-active' : ''}
        >
          <UnderlineIcon />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleStrike().run()}
          className={editorState.isStrike ? 'is-active' : ''}
        >
          <StrikeIcon />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleHeading({ level: 1 }).run()}
          className={editorState.isHeading1 ? 'is-active' : ''}
        >
          <HeadingOneIcon />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleHeading({ level: 2 }).run()}
          className={editorState.isHeading2 ? 'is-active' : ''}
        >
          <HeadingTwoIcon />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleHeading({ level: 3 }).run()}
          className={editorState.isHeading3 ? 'is-active' : ''}
        >
          <ListIcon />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleOrderedList().run()}
          className={editorState.isOrderedList ? 'is-active' : ''}
        >
          <ListOrderedIcon />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().unsetAllMarks().clearNodes().run()}>
          Clear formatting
        </button>
        <button type="button" onClick={() => editor.chain().focus().undo().run()} disabled={!editorState.canUndo}>
          <Undo2Icon />
        </button>
        <button type="button" onClick={() => editor.chain().focus().redo().run()} disabled={!editorState.canRedo}>
          <Redo2Icon />
        </button>
      </div>
    </div>
  )
}
