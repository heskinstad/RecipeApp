import { useState } from 'react';
import './style.css';

const Collapsible = (props) => {
    const [open, setOPen] = useState(false);

    const toggle = () => {
        setOPen(!open);
    };
      
    return (
        <div>
          <div className="category-link" onClick={toggle}>{props.label}</div>
          {open && (
            <div>
                {props.children}
            </div>
          )}
        </div>
      );
};

export default Collapsible;