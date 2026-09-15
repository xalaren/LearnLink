interface ISideNavProps {
    className?: string;
    children: React.ReactNode;
}

function SideNav({ children, className = '' }: ISideNavProps) {
    return (
        <nav className={`side-nav ${className}`}>
            {children}
        </nav >
    );
}

export default SideNav;