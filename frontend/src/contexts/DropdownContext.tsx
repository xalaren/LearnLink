import React, { createContext, useState } from 'react'

interface IDropdownContext {
    active: boolean;
    toggle: () => void;
    deselect: () => void;
}

export const DropdownContext = createContext<IDropdownContext>({
    active: false,
    toggle: () => { },
    deselect: () => { }
})

export const DropdownState = ({ children }: { children: React.ReactNode }) => {
    const [active, setActive] = useState(false);

    const toggle = () => setActive(prev => !prev);
    const deselect = () => setActive(false);

    return (
        <DropdownContext.Provider value={{ active, toggle, deselect }}>
            {children}
        </DropdownContext.Provider>
    )
}