import { ILinkData } from "../models/interfaces";
import { NavButton } from "../ui/NavButton";
interface IHeaderNavButtonsProps {
    links: ILinkData[];
}

export function HeaderNavButtons({ links }: IHeaderNavButtonsProps) {

    return (
        <nav className="header__nav">
            <div className="header__buttons">
                {
                    links.map(item => <NavButton link={item.path} key={links.indexOf(item)}>{item.title}</NavButton>)
                }
            </div>
        </nav >
    );
}