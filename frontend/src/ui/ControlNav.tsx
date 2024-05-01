interface IControlNavProps {
    children: React.ReactNode;
}

function ControlNav({ children }: IControlNavProps) {
    return (
        <nav className="control-nav">
            {children}
        </nav>
    );
}

export default ControlNav;