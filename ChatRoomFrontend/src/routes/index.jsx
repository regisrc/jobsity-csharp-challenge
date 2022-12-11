import { Suspense, lazy } from 'react';
import { Routes, Route } from 'react-router-dom';

const Login = lazy(() => import('../pages/Login/index'));
const ChatRoom = lazy(() => import('../pages/ChatRoom/index'));
const Message = lazy(() => import('../pages/Message/index'));

const RoutesApp = () => (
  <Suspense>
    <Routes>
    <Route path="/" element={<Login />}></Route>
    <Route path="/home" element={<ChatRoom />}></Route>
    <Route path="/chatroom" element={<Message />}></Route>
    </Routes> 
  </Suspense>
);

export default RoutesApp;