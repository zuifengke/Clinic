using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Windy.WebMVC.Web2.EFDao
{
    public class RepositoryEnter
    {  /// <summary>
       /// 统一SaveChange方法
       /// </summary>
       /// <returns></returns>
        public int SaveChange()
        {
            return DbContextFactory.GetCurrentDbContext().SaveChanges();
        }

        /// <summary>
        /// 获取订房仓储
        /// </summary>
        public RoomOrderRepository RoomOrderRepository { get { return new RoomOrderRepository(); } }
        /// <summary>
        /// 获取员工仓储
        /// </summary>
        public EmployeeRepository EmployeeRepository { get { return new EmployeeRepository(); } }
        /// <summary>
        /// 获取菜单仓储
        /// </summary>
        public MenuRepository MenuRepository { get { return new MenuRepository(); } }
        /// <summary>
        /// 获取用户菜单仓储
        /// </summary>
        public EmpMenuRepository EmpMenuRepository { get { return new EmpMenuRepository(); } }
        /// <summary>
        /// 获取角色菜单仓储
        /// </summary>
        public RoleMenuRepository RoleMenuRepository { get { return new RoleMenuRepository(); } }
        /// <summary>
        /// 获取考点仓储
        /// </summary>
        public ExamPlaceRepository ExamPlaceRepository { get { return new ExamPlaceRepository(); } }
        /// <summary>
        /// 获取用户机构仓储
        /// </summary>
        public EmpOrgRepository EmpOrgRepository { get { return new EmpOrgRepository(); } }
        /// <summary>
        /// 获取用户角色仓储
        /// </summary>
        public EmpRoleRepository EmpRoleRepository { get { return new EmpRoleRepository(); } }
        /// <summary>
        /// 获取机构仓储
        /// </summary>
        public OrgnizationRepository OrgnizationRepository { get { return new OrgnizationRepository(); } }
        /// <summary>
        /// 获取会员仓储
        /// </summary>
        public UsersRepository UsersRepository { get { return new UsersRepository(); } }
        /// <summary>
        /// 获取问题反馈仓储
        /// </summary>
        public DemandRepository DemandRepository { get { return new DemandRepository(); } }
        /// <summary>
        /// 获取公告仓储
        /// </summary>
        public NewsRepository NewsRepository { get { return new NewsRepository(); } }
        /// <summary>
        /// 获取日志仓储
        /// </summary>
        public LogRepository LogRepository { get { return new LogRepository(); } }
        /// <summary>
        /// 获取微信授权用户仓储
        /// </summary>
        public OAuthUserRepository OAuthUserRepository { get { return new OAuthUserRepository(); } }
        /// <summary>
        /// 获取角色仓储
        /// </summary>
        public RoleRepository RoleRepository { get { return new RoleRepository(); } }
        /// <summary>
        /// 获取类别仓储
        /// </summary>
        public CategoryRepository CategoryRepository { get { return new CategoryRepository(); } }
        /// <summary>
        /// 获取文章仓储
        /// </summary>
        public ArticleRepository ArticleRepository { get { return new ArticleRepository(); } }
        /// <summary>
        /// 获取留言建议仓储
        /// </summary>
        public AdviceRepository AdviceRepository { get { return new AdviceRepository(); } }
        /// <summary>
        /// 获取会员仓储
        /// </summary>
        public MemberRepository MemberRepository { get { return new MemberRepository(); } }
        /// <summary>
        /// 获取书籍仓储
        /// </summary>
        public BookRepository BookRepository { get { return new BookRepository(); } }
        /// <summary>
        /// 获取QQ授权用户
        /// </summary>
        public QQUserRepository QQUserRepository { get { return new QQUserRepository(); } }
        /// <summary>
        /// 获取博客仓储
        /// </summary>
        public BlogRepository BlogRepository { get { return new BlogRepository(); } }
        /// <summary>
        /// 获取今日头条仓储
        /// </summary>
        public ToutiaoRepository ToutiaoRepository { get { return new ToutiaoRepository(); } }
        /// <summary>
        /// 获取培训视频仓储
        /// </summary>
        public TrainRepository TrainRepository { get { return new TrainRepository(); } }
        /// <summary>
        /// 获取酒店仓储
        /// </summary>
        public HotelRepository HotelRepository { get { return new HotelRepository(); } }
        /// <summary>
        /// 获取广告仓储
        /// </summary>
        public AdvertRepository AdvertRepository { get { return new AdvertRepository(); } }
        /// <summary>
        /// 获取产品仓储
        /// </summary>
        public ProductRepository ProductRepository { get { return new ProductRepository(); } }
        /// <summary>
        /// 获取邀请仓储
        /// </summary>
        public InviteRepository InviteRepository { get { return new InviteRepository(); } }
        /// <summary>
        /// 获取抽奖仓储
        /// </summary>
        public LotteryRepository LotteryRepository { get { return new LotteryRepository(); } }
        /// <summary>
        /// 获取药品仓储
        /// </summary>
        public DrugRepository DrugRepository { get { return new DrugRepository(); } }

    }
}