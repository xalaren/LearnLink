import { useNavigate } from "react-router-dom";

interface INavLinkProps {
    link: string;
    children?: React.ReactNode;
    className?: string;
}

export function NavLink({ link, children, className }: INavLinkProps) {
    const navigate = useNavigate();
    const classes = className || 'white-link';

    return (
        <a className={classes} onClick={() => navigate(link)}>{children}</a>
    )
}