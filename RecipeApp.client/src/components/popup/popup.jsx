import { useState } from "react";
import "./popup.css";

function Popup() {
    const [isOpen, setIsOpen] = useState(false);

    const togglePopup = () => {
        setIsOpen(!isOpen);
    };

    return (
        <>
            {isOpen && (
                <div className="popup">
                    <div className="popup-content">
                        <span className="close" onClick={togglePopup}>&times;</span>
                        <p>This is a popup!</p>
                    </div>
                </div>
            )}
        </>
    )
}

export default Popup;