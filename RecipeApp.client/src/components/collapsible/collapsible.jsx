import { useState } from 'react';

const Collapsible = ({label, children, open: controlledOpen, setOpen: setControlledOpen}) => {
    const [uncontrolledOpen, setUncontrolledOpen] = useState(false);

    const isControlled = controlledOpen !== undefined && setControlledOpen !== undefined;
    const open = isControlled ? controlledOpen : uncontrolledOpen;
    const setOpen = isControlled ? setControlledOpen : setUncontrolledOpen;

    const toggle = () => {
        setOpen(!open);
    };
      
    return (
        <div>
          <div className="category-link" onClick={toggle}>{label}</div>
          {open && (
            <div>
                {children}
            </div>
          )}
        </div>
      );
};

export default Collapsible;