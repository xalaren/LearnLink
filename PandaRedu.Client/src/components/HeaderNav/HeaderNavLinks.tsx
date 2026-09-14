import { ILinkData } from "../../models/interfaces";
import { NavLink } from "../NavLinksAndButtons/NavLink";

interface IHeaderNavLinksProps {
    links: ILinkData[];
}

export function HeaderNavLinks({ links }: IHeaderNavLinksProps) {
    return (
        <nav className="header__nav">
            {
                links.map(link => <NavLink link={link.path}>{link.title}</NavLink>)
            }
        </nav>
    );
}