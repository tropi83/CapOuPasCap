/* tslint:disable:max-line-length */
import { FuseNavigationItem } from '@fuse/components/navigation';

export const defaultNavigation: FuseNavigationItem[] = [
    {
        id   : 'example',
        title: 'Example',
        type : 'basic',
        icon : 'heroicons_outline:chart-pie',
        link : '/example'
    }
];

export const futuristicNavigation: FuseNavigationItem[] = [
    {
        id: 'admin_copc',
        title: 'ADMINISTRATION CAP OU PAS CAP',
        subtitle: '',
        type: 'group',
        icon: 'heroicons_outline:document',
        children: [
            {
                id: 'copc',
                title: 'Accueil',
                type: 'basic',
                icon: 'heroicons_solid:home',
                link: '/copc',
            },
        ]
    }
];

