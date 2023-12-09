import { useNavigate } from "react-router-dom";

interface INavButtonProps {
    link: string;
    children?: React.ReactNode;
    className?: string;
}

export function NavButton({ link, children, className }: INavButtonProps) {
    const navigate = useNavigate();
    const classes = className || 'white-tp-button';

    return (
        <button className={classes} onClick={() => navigate(link)}>{children}</button>
    )
}