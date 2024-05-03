interface ISideNavLinkProps {
    text: string;
    selected: boolean;
    onClick: () => void;
    iconClassName?: string;
}

function SideNavLink({ text, selected, onClick, iconClassName = '' }: ISideNavLinkProps) {
    return (
        <div className={`side-nav__nav-link nav-link ${selected && 'nav-link-selected'}`} onClick={onClick}>
            <span className={`nav-link__icon ${iconClassName}`}></span>
            {text}
        </div>
    );
}

export default SideNavLink;