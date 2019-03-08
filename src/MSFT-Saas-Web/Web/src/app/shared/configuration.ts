import { environment } from '../../environments/environment';

let endpoints = {}
endpoints[environment.RDBROKER_URL] = environment.RESOURCE_ID;

export const adalConfig = {
    tenant: 'common',
    clientId: environment.APPLICATION_ID,
    redirectUri: `${window.location.protocol}//${window.location.hostname}:${window.location.port}/postlogin`,
    endpoints: endpoints,
    cacheLocation: 'sessionStorage',
    popUp: false,
    postLogoutRedirectUri: `${window.location.protocol}//${window.location.hostname}:${window.location.port}/`,
};