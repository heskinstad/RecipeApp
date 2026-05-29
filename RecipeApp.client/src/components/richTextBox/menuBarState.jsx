export function menuBarStateSelector(ctx) {
  const editor = ctx.editor
  if (!editor) {
    return {}
  }
  
  return {
    isBold: ctx.editor.isActive('bold') ?? false,
    isItalic: ctx.editor.isActive('italic') ?? false,
    isUnderline: ctx.editor.isActive('underline') ?? false,
    isStrike: ctx.editor.isActive('strike') ?? false,

    // Block types
    isParagraph: ctx.editor.isActive('paragraph') ?? false,
    isHeading1: ctx.editor.isActive('heading', { level: 1 }) ?? false,
    isHeading2: ctx.editor.isActive('heading', { level: 2 }) ?? false,

    // Lists and blocks
    isBulletList: ctx.editor.isActive('bulletList') ?? false,
    isOrderedList: ctx.editor.isActive('orderedList') ?? false,

    canUndo: ctx.editor.can().undo() ?? false,
    canRedo: ctx.editor.can().redo() ?? false,
  }
}
