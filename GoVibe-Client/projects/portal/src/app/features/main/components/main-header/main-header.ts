import { CommonModule } from '@angular/common';
import { Component, computed } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Button } from '@components';
import { ThemeService } from '../../../../core/services/theme.service';
import { Icons } from "@icons";

interface User {
    name: string;
    email: string;
    avatar: string;
    isLoggedIn: boolean;
}

@Component({
    selector: 'app-main-header',
    imports: [CommonModule, RouterModule, Button, Icons],
    templateUrl: './main-header.html',
    styleUrl: './main-header.css',
})
export class MainHeader {
    // Dữ liệu mẫu
    user: User = {
        name: 'Nguyễn Văn A',
        email: 'nguyenvana@example.com',
        avatar: 'https://randomuser.me/api/portraits/men/1.jpg',
        isLoggedIn: false, // Thay đổi thành true để test trạng thái đã đăng nhập
    };

    showUserMenu = false;
    randomAvatar = 'https://randomuser.me/api/portraits/men/random.jpg';

    // Menu items
    menuItems = [
        { label: 'Home', link: '/', icon: '🏠' },
        { label: 'Contact', link: '/contact', icon: '📞' },
        { label: 'Suggestions', link: '/suggestions', icon: '💡' },
    ];

    isDark = computed(() => this.themeService.themeValue() === 'dark');

    constructor(private themeService: ThemeService) {
        // Tạo ảnh ngẫu nhiên mỗi lần load
        const randomId = Math.floor(Math.random() * 100);
        this.randomAvatar = `https://randomuser.me/api/portraits/${Math.random() > 0.5 ? 'men' : 'women'}/${randomId}.jpg`;
    }

    toggleUserMenu() {
        this.showUserMenu = !this.showUserMenu;
    }

    loginWithGoogle() {
        // Giả lập đăng nhập với Google
        console.log('Đăng nhập với Google');
        this.user.isLoggedIn = true;
        this.user.name = 'Nguyễn Văn A';
        this.user.email = 'nguyenvana@gmail.com';
        this.user.avatar = 'https://randomuser.me/api/portraits/men/1.jpg';
        this.showUserMenu = false;
        alert('Đăng nhập thành công!');
    }

    logout() {
        // Giả lập đăng xuất
        console.log('Đăng xuất');
        this.user.isLoggedIn = false;
        this.user.name = '';
        this.user.email = '';
        this.user.avatar = '';
        this.showUserMenu = false;
        alert('Đã đăng xuất!');
    }

    goToProfile() {
        console.log('Đi đến trang profile');
        this.showUserMenu = false;
        // Điều hướng đến trang profile
    }

    toggleTheme() {
        if (this.isDark()) {
            this.themeService.setTheme('light');
        } else {
            this.themeService.setTheme('dark');
        }
    }
}
