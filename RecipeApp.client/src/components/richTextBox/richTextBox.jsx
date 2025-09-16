import isHotkey from 'is-hotkey'
import { useCallback, useMemo } from 'react'
import {
    Editor,
    Element as SlateElement,
    Transforms,
    createEditor,
} from 'slate'
import { withHistory } from 'slate-history'
import { Editable, Slate, useSlate, withReact } from 'slate-react'
import { Button, Icon, Toolbar } from './elements'
import './richTextBox.css';
import format_bold from '../../resources/text_format/format_bold_24dp_000000_FILL0_wght400_GRAD0_opsz24.svg';
import format_italic from '../../resources/text_format/format_italic_24dp_000000_FILL0_wght400_GRAD0_opsz24.svg';
import format_underlined from '../../resources/text_format/format_underlined_24dp_000000_FILL0_wght400_GRAD0_opsz24.svg';
import header_one from '../../resources/text_format/format_h1_24dp_000000_FILL0_wght400_GRAD0_opsz24.svg';
import header_two from '../../resources/text_format/format_h2_24dp_000000_FILL0_wght400_GRAD0_opsz24.svg';
import quote from '../../resources/text_format/format_quote_24dp_000000_FILL0_wght400_GRAD0_opsz24.svg';
import format_list_numbered from '../../resources/text_format/format_list_numbered_24dp_000000_FILL0_wght400_GRAD0_opsz24.svg';
import format_list_bulleted from '../../resources/text_format/format_list_bulleted_24dp_000000_FILL0_wght400_GRAD0_opsz24.svg';

const HOTKEYS = {
    'mod+b': 'bold',
    'mod+i': 'italic',
    'mod+u': 'underline',
    'mod+`': 'code',
}
const LIST_TYPES = ['numbered-list', 'bulleted-list']
const TEXT_ALIGN_TYPES = ['left', 'center', 'right', 'justify']

const initialValue = [
    {
        type: 'paragraph',
        children: [{ text: '' }],
    },
]

function RichTextBox() {
    const renderElement = useCallback(props => <Element {...props} />, [])
    const renderLeaf = useCallback(props => <Leaf {...props} />, [])
    const editor = useMemo(() => withHistory(withReact(createEditor())), [])

    return (
        <Slate editor={editor} initialValue={initialValue}>
            <Toolbar className="richTextBox_toolbar">
                <MarkButton format="bold" icon={format_bold} />
                <MarkButton format="italic" icon={format_italic} />
                <MarkButton format="underline" icon={format_underlined} />
                <BlockButton format="heading-one" icon={header_one} />
                <BlockButton format="heading-two" icon={header_two} />
                <BlockButton format="block-quote" icon={quote} />
                <BlockButton format="numbered-list" icon={format_list_numbered} />
                <BlockButton format="bulleted-list" icon={format_list_bulleted} />
            </Toolbar>
            <Editable
                renderElement={renderElement}
                renderLeaf={renderLeaf}
                placeholder="Enter some rich text…"
                spellCheck
                autoFocus
                className="richTextBox_textbox"
                onKeyDown={event => {
                for (const hotkey in HOTKEYS) {
                    if (isHotkey(hotkey, event)) {
                    event.preventDefault()
                    const mark = HOTKEYS[hotkey]
                    toggleMark(editor, mark)
                    }
                }
                }}
            />
            </Slate>
    );
}

const toggleBlock = (editor, format) => {
  const isActive = isBlockActive(
    editor,
    format,
    isAlignType(format) ? 'align' : 'type'
  )
  const isList = isListType(format)
  Transforms.unwrapNodes(editor, {
    match: n =>
      !Editor.isEditor(n) &&
      SlateElement.isElement(n) &&
      isListType(n.type) &&
      !isAlignType(format),
    split: true,
  })
  let newProperties
  if (isAlignType(format)) {
    newProperties = {
      align: isActive ? undefined : format,
    }
  } else {
    newProperties = {
      type: isActive ? 'paragraph' : isList ? 'list-item' : format,
    }
  }
  Transforms.setNodes(editor, newProperties)
  if (!isActive && isList) {
    const block = { type: format, children: [] }
    Transforms.wrapNodes(editor, block)
  }
}
const toggleMark = (editor, format) => {
  const isActive = isMarkActive(editor, format)
  if (isActive) {
    Editor.removeMark(editor, format)
  } else {
    Editor.addMark(editor, format, true)
  }
}
const isBlockActive = (editor, format, blockType = 'type') => {
  const { selection } = editor
  if (!selection) return false
  const [match] = Array.from(
    Editor.nodes(editor, {
      at: Editor.unhangRange(editor, selection),
      match: n => {
        if (!Editor.isEditor(n) && SlateElement.isElement(n)) {
          if (blockType === 'align' && isAlignElement(n)) {
            return n.align === format
          }
          return n.type === format
        }
        return false
      },
    })
  )
  return !!match
}
const isMarkActive = (editor, format) => {
  const marks = Editor.marks(editor)
  return marks ? marks[format] === true : false
}
const Element = ({ attributes, children, element }) => {
  const style = {}
  if (isAlignElement(element)) {
    style.textAlign = element.align
  }
  switch (element.type) {
    case 'block-quote':
      return (
        <blockquote style={style} {...attributes}>
          {children}
        </blockquote>
      )
    case 'bulleted-list':
      return (
        <ul style={style} {...attributes}>
          {children}
        </ul>
      )
    case 'heading-one':
      return (
        <h1 style={style} {...attributes}>
          {children}
        </h1>
      )
    case 'heading-two':
      return (
        <h2 style={style} {...attributes}>
          {children}
        </h2>
      )
    case 'list-item':
      return (
        <li style={style} {...attributes}>
          {children}
        </li>
      )
    case 'numbered-list':
      return (
        <ol style={style} {...attributes}>
          {children}
        </ol>
      )
    default:
      return (
        <p style={style} {...attributes}>
          {children}
        </p>
      )
  }
}
const Leaf = ({ attributes, children, leaf }) => {
  if (leaf.bold) {
    children = <strong>{children}</strong>
  }
  if (leaf.code) {
    children = <code>{children}</code>
  }
  if (leaf.italic) {
    children = <em>{children}</em>
  }
  if (leaf.underline) {
    children = <u>{children}</u>
  }
  return <span {...attributes}>{children}</span>
}
const BlockButton = ({ format, icon }) => {
  const editor = useSlate()
  return (
    <Button
      className='richTextBox_elements_button'
      active={isBlockActive(
        editor,
        format,
        isAlignType(format) ? 'align' : 'type'
      )}
      onMouseDown={event => {
        event.preventDefault()
        toggleBlock(editor, format)
      }}
    >
      <img src={icon} alt={format} />
    </Button>
  )
}
const MarkButton = ({ format, icon }) => {
  const editor = useSlate()
  return (
    <Button
      className='richTextBox_elements_button'
      active={isMarkActive(editor, format)}
      onMouseDown={event => {
        event.preventDefault()
        toggleMark(editor, format)
      }}
    >
      <img src={icon} alt={format} />
    </Button>
  )
}
const isAlignType = format => {
  return TEXT_ALIGN_TYPES.includes(format)
}
const isListType = format => {
  return LIST_TYPES.includes(format)
}
const isAlignElement = element => {
  return 'align' in element
}

export default RichTextBox;