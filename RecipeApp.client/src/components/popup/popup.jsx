import { useState } from "react";
import "./popup.css";

function Popup({message, handleAction, onClose, isOpen}) {
    const clickConfirm = () => {
        handleAction();
        onClose();
    }

    if (!isOpen)
        return null;

    return (
        <>
            {isOpen && (
            <dialog open={isOpen} className="popup">
                <div className="popup_content">
                    <h2>{message}</h2>
                    <div className="popup_buttons">
                        <button onClick={clickConfirm}>Confirm</button>
                        <button onClick={onClose}>Cancel</button>
                    </div>
                </div>
            </dialog>
            )}
        </>
    )
}

export default Popup;