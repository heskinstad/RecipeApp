
import React from 'react';
import ReactDOM from 'react-dom';
import './elements.css';

export const Button = React.forwardRef(
  ({ className, active, reversed, ...props }, ref) => (
    <span
      {...props}
      ref={ref}
      className="richTextBox_elements_button"
    />
  )
)
export const Icon = React.forwardRef(({ className, ...props }, ref) => (
  <span
    {...props}
    ref={ref}
    className="richTextBox_elements_icon"
  />
))
export const Instruction = React.forwardRef(({ className, ...props }, ref) => (
  <div
    {...props}
    ref={ref}
    className="richTextBox_elements_instruction"
  />
))
export const Menu = React.forwardRef(({ className, ...props }, ref) => (
  <div
    {...props}
    data-test-id="menu"
    ref={ref}
    className="richTextBox_elements_menu"
  />
))
export const Portal = ({ children }) => {
  return typeof document === 'object'
    ? ReactDOM.createPortal(children, document.body)
    : null
}
export const Toolbar = React.forwardRef(({ className, ...props }, ref) => (
  <Menu
    {...props}
    ref={ref}
    className="richTextBox_elements_toolbar"
  />
))