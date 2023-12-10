import { ILinkData } from "../helpers/interfaces";
import { NavButton } from "../ui/NavButton";
interface IHeaderNavButtonsProps {
    links: ILinkData[];
}

export function HeaderNavButtons({ links }: IHeaderNavButtonsProps) {

    return (
        <nav className="header__nav">
            {
                links.map(item => <NavButton link={item.path} key={links.indexOf(item)}>{item.title}</NavButton>)
            }
        </nav>
    );
}